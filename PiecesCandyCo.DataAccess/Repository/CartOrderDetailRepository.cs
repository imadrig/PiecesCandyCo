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
    public class CartOrderDetailRepository : Repository<CartOrderDetail>, ICartOrderDetailRepository
    {
        private ApplicationDbContext _db;
        public CartOrderDetailRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public void Update(CartOrderDetail cartOrderDetail)
        {
            _db.CartOrderDetails.Update(cartOrderDetail);
        }
    }
}
