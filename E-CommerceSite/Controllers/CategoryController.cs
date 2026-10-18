using E_CommerceSite.Datos;
using E_CommerceSite.Datos.Category;
using E_CommerceSite.Models;
using E_CommerceSite.Negocio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace E_CommerceSite.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IICanHandleCategories handleCategories;

        public CategoryController(IICanHandleCategories handleCategories)
        {
            this.handleCategories = handleCategories;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await handleCategories.GetAllAsNotTraking();

            var model = new CategoryViewModel()
            {
                Categories = categories.Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Constantes.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
            {
            
            if (!ModelState.IsValid || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description))
            {
                TempData["ErrorMessage"] = $"Ha ocurrido un error al crear la categoria. {model.Name}";
                return RedirectToAction(nameof(Index), model);
            }

            var category = new Category()
            {   
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description
            };


            var result = await handleCategories.Create(category);

            if (result > 0)
            {
                TempData["SuccessMessage"] = "Categoria creado correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = $"Ha ocurrido un error al crear la categoria. {model.Name}";
            }

            return RedirectToAction(nameof(Index),model);
        }

        [HttpPost]
        [Authorize(Constantes.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al intentar editar la categoria seleccionada.";
                return RedirectToAction(nameof(Index));
            }

            //TODO: Terminar la secion de editar una categoria

            Category category = new Category()//creamos la entidad a enviar a la DB 
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description!
            };

            var result = await handleCategories.Edit(category);

            if (result > 0)
            {
                TempData["SuccessMessage"] = "Categia editada correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al intentar editar la categoria seleccionada.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Constantes.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if(id == Guid.Empty)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al eliminar la categoria.";
                return RedirectToAction(nameof(Index));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            var resultado = await handleCategories.Delete(id, userId);

            if(resultado > 0)
            {
                TempData["SuccessMessage"] = "La categoria ha sido eliminada correctamente";
            }
            else if(resultado == -3)
            {
                TempData["ErrorMessage"] = "La categoria tiene productos asociados, para poder eliminar esta, no debe estar asociadas a ningun producto.";
            }
            else
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al eliminar la categoria.";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
