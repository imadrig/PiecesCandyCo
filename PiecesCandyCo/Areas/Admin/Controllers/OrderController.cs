using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PiecesCandyCo.DataAccess.Repository.IRepository;
using PiecesCandyCo.Models;
using PiecesCandyCo.Models.ViewModels;

namespace PiecesCandyCo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

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
            OrderVM orderVM = new OrderVM()
            {
                CustomerOrderDetail = _unitOfWork.CustomerOrderDetail.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                CartOrderDetail = _unitOfWork.CartOrderDetail.GetAll(u => u.CustomerOrderDetailId == orderId, includeProperties: "Product")
            };
            return View(orderVM);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<CustomerOrderDetail> customerOrderDetails = _unitOfWork.CustomerOrderDetail.GetAll(includeProperties: "ApplicationUser").ToList();
            return Json(new { data = customerOrderDetails });
        }

        #endregion
    }
}
