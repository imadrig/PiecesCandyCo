using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PiecesCandyCo.DataAccess.Repository.IRepository;
using PiecesCandyCo.Models;
using PiecesCandyCo.Models.ViewModels;
using PiecesCandyCo.Utility;
using System.Security.Claims;

namespace PiecesCandyCo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {
            OrderVM = new()
            {
                CustomerOrderDetail = _unitOfWork.CustomerOrderDetail.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                CartOrderDetail = _unitOfWork.CartOrderDetail.GetAll(u => u.CustomerOrderDetailId == orderId, includeProperties: "Product")
            };
            return View(OrderVM);
        }


        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult UpdateOrderDetail()
        {
            var customerOrderDetailFromDb = _unitOfWork.CustomerOrderDetail.Get(u => u.Id == OrderVM.CustomerOrderDetail.Id);
            customerOrderDetailFromDb.Name = OrderVM.CustomerOrderDetail.Name;
            customerOrderDetailFromDb.PhoneNumber = OrderVM.CustomerOrderDetail.PhoneNumber;
            customerOrderDetailFromDb.StreetAddress = OrderVM.CustomerOrderDetail.StreetAddress;
            customerOrderDetailFromDb.City = OrderVM.CustomerOrderDetail.City;
            customerOrderDetailFromDb.State = OrderVM.CustomerOrderDetail.State;
            customerOrderDetailFromDb.ZipCode = OrderVM.CustomerOrderDetail.ZipCode;

            if (!string.IsNullOrEmpty(OrderVM.CustomerOrderDetail.Carrier))
            {
                customerOrderDetailFromDb.Carrier = OrderVM.CustomerOrderDetail.Carrier;
            }
            if (!string.IsNullOrEmpty(OrderVM.CustomerOrderDetail.TrackingNumber))
            {
                customerOrderDetailFromDb.TrackingNumber = OrderVM.CustomerOrderDetail.TrackingNumber;
            }
            _unitOfWork.CustomerOrderDetail.Update(customerOrderDetailFromDb);
            _unitOfWork.Save();

            TempData["Success"] = "Order Details Updated Succesfully!";

            return RedirectToAction(nameof(Details), new { orderId = customerOrderDetailFromDb.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult ShipOrder()
        {
            var customerOrderDetail = _unitOfWork.CustomerOrderDetail.Get(u => u.Id == OrderVM.CustomerOrderDetail.Id);
            customerOrderDetail.TrackingNumber = OrderVM.CustomerOrderDetail.TrackingNumber;
            customerOrderDetail.Carrier = OrderVM.CustomerOrderDetail.Carrier;
            customerOrderDetail.OrderStatus = SD.StatusShipped;
            customerOrderDetail.ShipDate = DateTime.Now;

            _unitOfWork.CustomerOrderDetail.Update(customerOrderDetail);
            _unitOfWork.Save(); 
            TempData["Success"] = "Order Shipped Succesfully!";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.CustomerOrderDetail.Id });
        }


        #region API CALLS

        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            IEnumerable<CustomerOrderDetail> customerOrderDetails1;

            if (User.IsInRole(SD.Role_Admin))
            {
                customerOrderDetails1 = _unitOfWork.CustomerOrderDetail.GetAll(includeProperties: "ApplicationUser").ToList();

            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                customerOrderDetails1 = _unitOfWork.CustomerOrderDetail.GetAll(u => u.ApplicationUserId == userId, includeProperties: "ApplicationUser");
            }
            return Json (new {data = customerOrderDetails1});
        }

        #endregion
    }
}
