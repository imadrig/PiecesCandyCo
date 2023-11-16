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
    public class ShoppingCart
    {
        public int Id { get; set; }

        public int ProductId {  get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

        [Range(1, 2000, ErrorMessage = "Please enter a value between 1 and 2,000")]
        public int Quantity {  get; set; }
        
        public string ApplicationUserId {  get; set; }
        
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

            [NotMapped]
            public double Price { get; set; }
    }
}
