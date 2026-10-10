

using Microsoft.EntityFrameworkCore;

namespace E_CommerceSite.Datos.Product
{
    public class Product
    {

        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } =  string.Empty;

        public string Description { get; set; } = string.Empty;

        [Precision(18,2)]
        public decimal Price { get; set; }

        public int Stock {  get; set; } = 0;


        //Category

        public Category.Category Category { get; set; } = null!;    

    }
}
