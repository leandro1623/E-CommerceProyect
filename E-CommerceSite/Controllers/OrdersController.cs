using Azure.Core;
using E_CommerceSite.Datos.Entities.OrderNameSpace;
using E_CommerceSite.Datos.OrderNameSpace;
using E_CommerceSite.Datos.Product;
using E_CommerceSite.Models;
using E_CommerceSite.Models.Order;
using E_CommerceSite.Negocio.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace E_CommerceSite.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ICanHandleCustomers handleCustomers;
        private readonly IICanHandleProducts handleProducts;
        private readonly IICanHandleOrders handleOrders;

        public OrdersController(ICanHandleCustomers handleCustomers, IICanHandleProducts handleProducts, IICanHandleOrders handleOrders)
        {
            this.handleCustomers = handleCustomers;
            this.handleProducts = handleProducts;
            this.handleOrders = handleOrders;
        }
        public async Task<IActionResult> Index()
        {
            var customers = await handleCustomers.GetCustomers();
            var products = await handleProducts.GetAllAsNotTraking();
            var orders = await handleOrders.GetAllAsNotTraking();

            // TODO: aun no hay ordenes en la DB se tienen que crear
            var model = new OrderDetailsViewModel()
            {
                Customers = new CustomerViewModel()
                {
                    Customers = customers!.Select(c => new CustomerViewModel()
                    {
                        Id = c.Id,
                        Name = c.UserEntity.Fullname!
                    }).ToList()
                },

                Products = new ProductViewModel()
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
                            Id = p.Category.Id,
                            Name = p.Category.Name,
                            Description = p.Description
                        }

                    }).ToList()
                },

                OrderList = orders.Select(o => new OrderDetailsViewModel()
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    Customers = new CustomerViewModel()
                    {
                        Name = o.Customer.UserEntity.Fullname!,
                    },
                    OrderDate = o.OrderDate,
                    Total = o.Total,
                    Status = o.Status,
                }).ToList()

            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var order = await handleOrders.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            var model = new OrderDetailsViewModel
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                Subtotal = order.Subtotal,
                Discount = order.Discount,
                Tax = order.Tax,
                Total = order.Total,
                Status = order.Status,
                Customers = new CustomerViewModel
                {
                    Id = order.CustomerId,
                    Name = order.Customer?.UserEntity?.Fullname ?? "Sin cliente",
                    Email = order.Customer?.UserEntity?.Email ?? string.Empty,
                    Phone = order.Customer?.UserEntity?.PhoneNumber,
                    Address = order.Customer?.Address
                },
                Items = order.Items.Select(item => new OrderItemDetailsViewModel
                {
                    ProductId = item.ProductId,
                    ProductName = string.IsNullOrWhiteSpace(item.ProductName)
                        ? item.Product?.Name ?? "Producto no disponible"
                        : item.ProductName,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity
                }).ToList()
            };

            // El JSON coincide con los campos que muestra DetailsModal.
            return Json(new
            {
                model.Id,
                model.OrderNumber,
                model.OrderDate,
                model.Subtotal,
                model.Discount,
                model.Tax,
                model.Total,
                Status = model.Status.ToString(),
                CustomerName = model.Customers.Name,
                CustomerEmail = model.Customers.Email,
                CustomerPhone = model.Customers.Phone,
                CustomerAddress = model.Customers.Address,
                Products = model.Items.Select(item => new
                {
                    Name = item.ProductName,
                    item.UnitPrice,
                    item.Quantity,
                    Subtotal = item.LineTotal
                })
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, OrderStatus? status)
        {
            if (!ModelState.IsValid || id == Guid.Empty || !status.HasValue ||
                !Enum.IsDefined(typeof(OrderStatus), status.Value))
            {
                TempData["ErrorMessage"] = "Selecciona un pedido y un estado válidos.";
                return RedirectToAction(nameof(Index));
            }

            if (await handleOrders.ChangeStatus(id, status.Value))
            {
                TempData["SuccessMessage"] = "Estado del pedido actualizado.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se encontró el pedido para actualizar su estado.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid || id == Guid.Empty)
            {
                TempData["ErrorMessage"] = "No se pudo obtener el identificador del pedido.";
                return RedirectToAction(nameof(Index));
            }

            if (await handleOrders.Delete(id))
            {
                TempData["SuccessMessage"] = "Pedido eliminado correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se encontró el pedido para eliminar.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderDetailsViewModel model)
        {

            //if (!ModelState.IsValid) Siempre sera falso por que en el modelo que se recibe no todos los campos tienen valores,
            //se validaran los campos necesarios de manera manual
            //{
            //    return View(model);
            //}

            var selectedProducts = model.Products?.Products?
                   .Where(product =>
                       product.Id != Guid.Empty &&
                       product.Quantity > 0)
                   .GroupBy(product => product.Id)
                   .Select(group => new
                   {
                       ProductId = group.Key,
                       Quantity = group.Sum(product => product.Quantity)
                   })
                   .ToList() ?? [];

            if (model.Customers == null ||
                model.Customers.Id == Guid.Empty)
            {
                TempData["ErrorMessage"] = "Debes seleccionar un cliente.";
                return RedirectToAction(nameof(Index));
            }

            if (selectedProducts.Count == 0)
            {
                TempData["ErrorMessage"] =
                    "Debes seleccionar al menos un producto.";

                return RedirectToAction(nameof(Index));
            }

            var Products = await handleProducts.GetAllAsNotTraking();

            var productIds = selectedProducts
                .Select(product => product.ProductId)
                .ToList();

            // Obtén los productos reales desde la base de datos.
            var databaseProducts = Products
                .Where(product => productIds.Contains(product.Id)).ToDictionary(product => product.Id);

            decimal subtotal = 0;
            var orderItems = new List<OrderItem>();

            foreach (var selectedProduct in selectedProducts)
            {
                if (!databaseProducts.TryGetValue(
                        selectedProduct.ProductId,
                        out var databaseProduct))
                {
                    TempData["ErrorMessage"] =
                        "Uno de los productos seleccionados no existe.";

                    return RedirectToAction(nameof(Index));
                }

                if (selectedProduct.Quantity > databaseProduct.Stock)
                {
                    TempData["ErrorMessage"] =
                        $"No hay suficiente existencia de {databaseProduct.Name}.";

                    return RedirectToAction(nameof(Index));
                }

                decimal unitPrice = databaseProduct.Price;

                decimal itemSubtotal = decimal.Round(
                    unitPrice * selectedProduct.Quantity,
                    2,
                    MidpointRounding.AwayFromZero);

                subtotal += itemSubtotal;

                orderItems.Add(new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = databaseProduct.Id,
                    ProductName = databaseProduct.Name,
                    Quantity = selectedProduct.Quantity,
                    UnitPrice = unitPrice,
                    LineTotal = itemSubtotal,
                    Subtotal = itemSubtotal
                });
            }

            decimal discount = 0; // Coloca aquí tu regla de descuento. Por el momento no se estan manejando descuentos.

            decimal taxableAmount = subtotal - discount;

            // Ejemplo con un impuesto del 18 %.
            decimal tax = decimal.Round(
                taxableAmount * 0.18m,
                2,
                MidpointRounding.AwayFromZero);

            decimal total = taxableAmount + tax;

            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = model.Customers.Id,
                OrderDate = DateTime.UtcNow,
                OrderNumber =
                    $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}"[..21],
                Status = OrderStatus.Pending,

                Subtotal = subtotal,
                Discount = discount,
                Tax = tax,
                Total = total,

                Items = orderItems
            };

            var result = await handleOrders.Create(order);

            if (result > 0)
            {
                TempData["SuccessMessage"] = "Pedido creado correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Ha ocurido un error al crear el pedido.";
            }


            return RedirectToAction(nameof(Index));
        }

    }
}
