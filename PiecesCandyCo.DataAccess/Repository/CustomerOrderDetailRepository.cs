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
    }
}
