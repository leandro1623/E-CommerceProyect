using E_CommerceSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSite.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            var products = new List<ProductViewModel>();

            return View(products);
        }
    }
}
