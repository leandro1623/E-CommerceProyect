

namespace E_CommerceSite.Datos.Product
{
    public class Product
    {
        public Product()
        {
            Category = new Category.Category()
            {
                Name = "Test",
            };
        }

        public Guid Id { get; set; }
        public string Name { get; set; } =  string.Empty;

        public double Price { get; set; } = 0;

        public int Stock {  get; set; } = 0;


        //Category
        public Category.Category Category { get; set; }

    }
}
