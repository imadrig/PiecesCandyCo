using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiecesCandyCo.Models
{
    public class CartOrderDetail
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(CustomerOrderDetailId))]
        [ValidateNever]
        public int CustomerOrderDetailId { get; set; }

        [Required]
        [ForeignKey(nameof(ProductId))]
        [ValidateNever]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }


    }

}
