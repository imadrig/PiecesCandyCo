using PiecesCandyCo.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiecesCandyCo.Models;
using PiecesCandyCo.Models.ViewModels;
using System.Security.Claims;
using PiecesCandyCo.Utility;
using Stripe;
using Stripe.Checkout;

namespace PiecesCandyCo.Areas.Customer.Controllers
{
    [Area("customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartItems = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                CustomerOrderDetail = new()
            };

            foreach (var cart in ShoppingCartVM.ShoppingCartItems)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.CustomerOrderDetail.OrderTotal += (cart.Price * cart.Quantity);
            }

            return View(ShoppingCartVM);
        }

        public IActionResult CartCheckout()
        {


            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartItems = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                CustomerOrderDetail = new()
            };

            ShoppingCartVM.CustomerOrderDetail.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            ShoppingCartVM.CustomerOrderDetail.Name = ShoppingCartVM.CustomerOrderDetail.ApplicationUser.Name;
            ShoppingCartVM.CustomerOrderDetail.PhoneNumber = ShoppingCartVM.CustomerOrderDetail.ApplicationUser.PhoneNumber;
            ShoppingCartVM.CustomerOrderDetail.StreetAddress = ShoppingCartVM.CustomerOrderDetail.ApplicationUser.StreetAddress;
            ShoppingCartVM.CustomerOrderDetail.City = ShoppingCartVM.CustomerOrderDetail.ApplicationUser.City;
            ShoppingCartVM.CustomerOrderDetail.State = ShoppingCartVM.CustomerOrderDetail.ApplicationUser.State;
            ShoppingCartVM.CustomerOrderDetail.ZipCode = ShoppingCartVM.CustomerOrderDetail.ApplicationUser.ZipCode;

            foreach (var cart in ShoppingCartVM.ShoppingCartItems)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.CustomerOrderDetail.OrderTotal += (cart.Price * cart.Quantity);
            }

            return View(ShoppingCartVM);

        }

        [HttpPost]
        [ActionName(nameof(CartCheckout))]
        public IActionResult CartCheckoutPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartItems = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
            includeProperties: "Product");

            ShoppingCartVM.CustomerOrderDetail.OrderDate = DateTime.Now;
            ShoppingCartVM.CustomerOrderDetail.ApplicationUserId = userId;

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            foreach (var cart in ShoppingCartVM.ShoppingCartItems)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.CustomerOrderDetail.OrderTotal += (cart.Price * cart.Quantity);
            }

            if (applicationUser != null)
            {
                ShoppingCartVM.CustomerOrderDetail.OrderStatus = SD.StatusPending;
                ShoppingCartVM.CustomerOrderDetail.PaymentStatus = SD.PaymentStatusPending;
            }
            _unitOfWork.CustomerOrderDetail.Add(ShoppingCartVM.CustomerOrderDetail);
            _unitOfWork.Save();

            foreach (var item in ShoppingCartVM.ShoppingCartItems)
            {
                CartOrderDetail cartOrderDetail = new()
                {
                    ProductId = item.ProductId,
                    CustomerOrderDetailId = ShoppingCartVM.CustomerOrderDetail.Id,
                    Price = item.Price,
                    Quantity = item.Quantity,

                };
                _unitOfWork.CartOrderDetail.Add(cartOrderDetail);
                _unitOfWork.Save();
            }

            if (applicationUser != null)
            {

                var options = new SessionCreateOptions
                {

                    SuccessUrl = $"https://localhost:7004/customer/ShoppingCart/OrderConfirmation?id={ShoppingCartVM.CustomerOrderDetail.Id}",
                    CancelUrl = "https://localhost:7004/customer/ShoppingCart/Index",
                    LineItems = new List<SessionLineItemOptions>(), 
                    Mode = "payment",
                };

                foreach (var item in ShoppingCartVM.ShoppingCartItems)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name
                            }

                        },
                        Quantity = item.Quantity
                    };
                    options.LineItems.Add(sessionLineItem);
                }
                var service = new SessionService();
                Session session = service.Create(options);
                _unitOfWork.CustomerOrderDetail.UpdateStripePaymentId(ShoppingCartVM.CustomerOrderDetail.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }

            return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.CustomerOrderDetail.Id });

        }

        public IActionResult OrderConfirmation(int id)
        {
            CustomerOrderDetail customerOrderDetail = _unitOfWork.CustomerOrderDetail.Get(u => u.Id == id, includeProperties: "ApplicationUser");
            if (customerOrderDetail.PaymentStatus == SD.PaymentStatusPending)
            {
                var service = new SessionService();
                Session session = service.Get(customerOrderDetail.SessionId);
                
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.CustomerOrderDetail.UpdateStripePaymentId(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.CustomerOrderDetail.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }

            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == customerOrderDetail.ApplicationUserId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();


            return View(id);
        }

        public IActionResult AddToQuantity(int cartId)
        {
            var activeCart = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            activeCart.Quantity += 1;
            _unitOfWork.ShoppingCart.Update(activeCart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult SubtractFromQuantity(int cartId)
        {
            var activeCart = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId, tracked: true);

            if (activeCart.Quantity <= 1)
            {
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == activeCart.ApplicationUserId).Count() - 1);
                _unitOfWork.ShoppingCart.Remove(activeCart);
            }
            else
            {
                activeCart.Quantity -= 1;
                _unitOfWork.ShoppingCart.Update(activeCart);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var activeCart = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId, tracked:true);
            HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == activeCart.ApplicationUserId).Count() - 1);
            _unitOfWork.ShoppingCart.Remove(activeCart);
            
            _unitOfWork.Save();
            
            return RedirectToAction(nameof(Index));
        }

        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Quantity < 10)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                return shoppingCart.Product.Price10;
            }
        }
    }
}
