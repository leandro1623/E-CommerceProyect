using E_CommerceSite.Datos;
using E_CommerceSite.Datos.Product;
using E_CommerceSite.Models;
using E_CommerceSite.Negocio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_CommerceSite.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IICanHandleProducts handleProducts;
        private readonly IICanHandleCategories handleCategories;

        public ProductsController(IICanHandleProducts handleProducts, IICanHandleCategories handleCategories)
        {
            this.handleProducts = handleProducts;
            this.handleCategories = handleCategories;
        }

        public async Task<IActionResult> Index()
        {
            var products = await handleProducts.GetAllAsNotTraking();
            var categories = await handleCategories.GetAllAsNotTraking();

            var productsModel = new ProductViewModel()
            {
                Products = products.Select(p => new ProductViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    Category = new CategoryViewModel()
                    {
                        Id = p.CategoryId,
                        Description = p.Category.Description,
                        Name = p.Category.Name,
                    }
                }).ToList(),

                Category = new CategoryViewModel()
                {
                    Categories = categories.Select(c => new CategoryViewModel()
                    {
                        Id = c.Id,
                        Name= c.Name,
                        Description= c.Description
                    }).ToList()
                }
            };

            return View(productsModel);
        }

        [HttpPost]
        [Authorize(Constantes.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            //no se validaran estos campos del modelo Categry, solo se validara el campo Id de este modelo
            ModelState.Remove("Category.Name");
            ModelState.Remove("Category.Description");

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "No se pudo crear el producto";
                return RedirectToAction(nameof(Index),model);
            }

            var result = await handleProducts.Create(new Datos.Product.Product()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                CategoryId = model.Category.Id,
            });

            if(result > 0)
            {
                TempData["Success"] = $"Se ha creado el producto {model.Name} correctamente";
            }
            else
            {
                TempData["Error"] = "No se pudo crear el producto";
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [Authorize(Constantes.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            //no se necesitan validar estos campos
            ModelState.Remove("Category.Name");
            ModelState.Remove("Category.Description");

            if (!ModelState.IsValid)
            {
                TempData["Errormessage"] = "Ha ocurrido un error al agregar el producto";
                return RedirectToAction(nameof(Index));
            }

            var product = new Product()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                CategoryId = model.Category.Id,
            };

            var result = await handleProducts.Edit(product);

            if(result > 0)
            {
                TempData["SuccessMessage"] = $"El producto {model.Name} ha sido editado correctamente";
            }
            else if(result == -1)
            {
                TempData["ErrorMessage"] = $"El producto {model.Name} no existe en la base de datos.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Constantes.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if(id == Guid.Empty)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error al intentar eliminar el producto";
                RedirectToAction(nameof(Index));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            var result = await handleProducts.Delete(id, userId);

            if(result > 0)
            {
                TempData["SuccessMessage"] = "El producto ha sido eliminado correctamente";
            }
            else if(result == -3)
            {
                TempData["ErrorMessage"] = "No se puedo eliminar el producto debido a que exisen ordenes pendientes o en proceso asociadas a este producto";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
