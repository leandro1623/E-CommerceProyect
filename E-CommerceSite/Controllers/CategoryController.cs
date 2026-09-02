using E_CommerceSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSite.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            var categories = new List<CategoryViewModel>();

            return View(categories);
        }
    }
}
