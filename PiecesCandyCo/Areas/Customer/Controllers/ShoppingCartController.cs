using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiecesCandyCo.DataAccess.Repository.IRepository;
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
                includeProperties: "Product")
            };

            foreach (var cart in ShoppingCartVM.ShoppingCartItems)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderTotal += (cart.Price * cart.Quantity);
            }

            return View(ShoppingCartVM);
        }

        public IActionResult CartCheckout()
        {
            return View();
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
