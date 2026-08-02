using E_CommerceSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSite.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {

            var customers = new List<Customer>();

            return View(customers);
        }
    }
}
