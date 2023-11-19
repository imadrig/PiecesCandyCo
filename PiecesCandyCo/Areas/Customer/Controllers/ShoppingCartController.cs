using PiecesCandyCo.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiecesCandyCo.Models;
using PiecesCandyCo.Models.ViewModels;
using System.Security.Claims;

namespace PiecesCandyCo.Areas.Customer.Controllers
{
    [Area("customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
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
            var activeCart = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            if (activeCart.Quantity <= 1)
            {
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
            var activeCart = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

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
