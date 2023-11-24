using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiecesCandyCo.Models.ViewModels
{
    public class OrderVM
    {
        public CustomerOrderDetail CustomerOrderDetail { get; set; }
        public IEnumerable<CartOrderDetail> CartOrderDetail { get; set; }
        


    }
}
