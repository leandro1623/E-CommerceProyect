using E_CommerceSite.Models;
using E_CommerceSite.Negocio.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_CommerceSite.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICanHandleCustomers handleCustomers;

        public CustomerController(ICanHandleCustomers handleCustomers)
        {
            this.handleCustomers = handleCustomers;
        }

        public async Task<IActionResult> Index(string m = "")
        {
            //if (!User.Identity!.IsAuthenticated)
            //{
            //    return RedirectToAction("Login", "Account");
            //}

            var customers = await handleCustomers.GetCustomers();

            CustomerViewModel model = new CustomerViewModel();

            model.Customers =  customers.Select(c => new CustomerViewModel()
            {
                Id = c.Id,
                Name = c.UserEntity!.Fullname,
                Address = c.Address??"No ha agregado direcciones",
                Email = c.UserEntity.Email!,
                Phone = c.UserEntity.PhoneNumber!,
                CreatedAt = c.CreateDate,
                IsActive = c.UserEntity.EmailConfirmed,
                Orders = c.Orders.Select(o=> new OrderViewModel() { Customer = null, CustomerId = o.Customer!.Id, Id = o.Id,
                OrderDate = o.OrderDate, Status = o.Status, Total = o.Total}).ToList(),
            }).ToList();

            

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid Id)
        {

            if (Id == Guid.Empty)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al eliminar al Usuario. No se ha podido obtener el id del usuario.";
                return View();
            }

            var currentUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)!;//Se toma el id del usuario actual, para comparar si se esta eliminando el mismo

            int deleted = await handleCustomers.Delete(Id, currentUserIdClaim);

            //int deleted = await handleCustomers.Delete(Id);

            if (deleted > 0)
            {
                TempData["SuccessMessage"] = "Usuario eliminado.";
            }
            else if (deleted == 0)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error desconocido al intentar eliminar al Usuario.";
            }
            else if (deleted == -1)
            {
                TempData["ErrorMessage"] = "No puedes eliminar tu propio usuario mientras estás logueado.";
            }
            else if(deleted == -2)
            {
                TempData["ErrorMessage"] = "No puedes eliminar tu propio usuario mientras estás logueado.";
            }
                        
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditarCustomerViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model); //TODO: tiene que crear un ViewModel para editar o anular la validacion de los campos que no son necesarios aca
            }

            int edited = await handleCustomers.EditUser(model.Id, model.Name, model.Email, model.Phone!, model.Address!);

            if(edited > 0)
            {
                TempData["SuccessMessage"] = "El usuario ha sido editado";
            }
            else
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al editar el usuario!";
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
