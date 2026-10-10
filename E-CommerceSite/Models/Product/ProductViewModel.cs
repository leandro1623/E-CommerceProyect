using E_CommerceSite.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceSite.Models
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } =  string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        
        [Precision(18,2)]
        public decimal Price { get; set; }


        public int Stock {  get; set; }

        public int Quantity { get; set; }

        //Category
        public CategoryViewModel Category { get; set; } = new CategoryViewModel();

        //list of product
        public List<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();

    }
}
