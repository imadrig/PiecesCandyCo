using PiecesCandyCo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiecesCandyCo.DataAccess.Repository.IRepository
{
    public interface ICartOrderDetailRepository : IRepository<CartOrderDetail>
    {
        void Update(CartOrderDetail cartOrderDetail);
        
    }
}
