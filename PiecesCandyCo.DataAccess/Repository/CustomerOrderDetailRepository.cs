using PiecesCandyCo.DataAccess.Data;
using PiecesCandyCo.DataAccess.Repository.IRepository;
using PiecesCandyCo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PiecesCandyCo.DataAccess.Repository
{
    public class CustomerOrderDetailRepository : Repository<CustomerOrderDetail>, ICustomerOrderDetailRepository 
    {
        private ApplicationDbContext _db;
        public CustomerOrderDetailRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public void Update(CustomerOrderDetail customerOrderDetail)
        {
            _db.CustomerOrderDetails.Update(customerOrderDetail);
        }

        public void UpdateStatus (int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _db.CustomerOrderDetails.FirstOrDefault(x => x.Id == id);

            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;

                if (string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId) 
        {
            var orderFromDb = _db.CustomerOrderDetails.FirstOrDefault(x => x.Id == id);
            if (!string.IsNullOrEmpty(sessionId))
            {
                orderFromDb.SessionId = sessionId;
            }
            if (!string.IsNullOrEmpty(paymentIntentId))
            {
                orderFromDb.PaymentIntentId = paymentIntentId;
            }
        }
    }
}
