using E_CommerceSite.DATA;
using E_CommerceSite.Datos.OrderNameSpace;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace E_CommerceSite.Negocio.Services
{

    public interface IICanHandleOrders
    {
        Task<int> Create(Order order);
        Task<List<Order>> GetAll();
        Task<List<Order>> GetAllAsNotTraking();
        Task<Order?> GetById(Guid id);
        Task<bool> ChangeStatus(Guid id, OrderStatus status);
        Task<bool> Delete(Guid id);
        Task<List<MonthlySaleViewModel>> GetMonthlySales(int numberOfMonths);
        Task<List<OrderStatusSummaryViewModel>> GetOrdersByStatus();
        Task<int> GetTotalCount();
        Task<decimal> GetTotalSales();
    }

    public class ICanHandleOrders : IICanHandleOrders
    {
        private readonly ApplicationDbContext dbContext;

        public ICanHandleOrders(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Order>> GetAll()
        {
            var orders = await dbContext.Orders.Include(o => o.Customer.UserEntity).ToListAsync();

            return orders;
        }

        public async Task<List<Order>> GetAllAsNotTraking()
        {
            var orders = await dbContext.Orders.Include(o => o.Customer.UserEntity).AsNoTracking().ToListAsync();

            return orders;
        }

        public async Task<int> Create(Order order)
        {
            dbContext.Orders.Add(order);

            return await dbContext.SaveChangesAsync();
        }

        public async Task<Order?> GetById(Guid id)
        {
            return await dbContext.Orders
                .AsNoTracking()
                .Include(order => order.Customer.UserEntity)
                .Include(order => order.Items)
                    .ThenInclude(item => item.Product)
                .SingleOrDefaultAsync(order => order.Id == id);
        }

        public async Task<bool> ChangeStatus(Guid id, OrderStatus status)
        {
            if (!Enum.IsDefined(typeof(OrderStatus), status))
            {
                return false;
            }

            var order = await dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            order.Status = status;
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            var order = await dbContext.Orders
                .Include(order => order.Items)
                .SingleOrDefaultAsync(order => order.Id == id);

            if (order == null)
            {
                return false;
            }

            dbContext.Orders.Remove(order);
            await dbContext.SaveChangesAsync();
            return true;
        }


        //IA

        public async Task<int> GetTotalCount()
        {
            return await dbContext.Orders.CountAsync();
        }

        public async Task<decimal> GetTotalSales()
        {
            return await dbContext.Orders
                .AsNoTracking()
                .Where(order => order.Status == OrderStatus.Completed)
                .Select(order => (decimal?)order.Total)
                .SumAsync() ?? 0;
        }

        public async Task<List<MonthlySaleViewModel>> GetMonthlySales(
            int numberOfMonths)
        {
            if (numberOfMonths <= 0)
            {
                numberOfMonths = 6;
            }

            DateTime currentDate = DateTime.Now;

            DateTime currentMonth = new(
                currentDate.Year,
                currentDate.Month,
                1
            );

            DateTime firstMonth =
                currentMonth.AddMonths(-(numberOfMonths - 1));

            var databaseResults = await dbContext.Orders
                .AsNoTracking()
                .Where(order =>
                    order.OrderDate >= firstMonth && order.Status == OrderStatus.Completed)
                .GroupBy(order => new
                {
                    order.OrderDate.Year,
                    order.OrderDate.Month
                })
                .Select(group => new
                {
                    group.Key.Year,
                    group.Key.Month,
                    Total = group.Sum(order => order.Total)
                })
                .ToListAsync();

            var culture = new CultureInfo("es-DO");

            var monthlySales =
                new List<MonthlySaleViewModel>();

            for (int index = 0;
                 index < numberOfMonths;
                 index++)
            {
                DateTime month =
                    firstMonth.AddMonths(index);

                var databaseResult =
                    databaseResults.FirstOrDefault(result =>
                        result.Year == month.Year &&
                        result.Month == month.Month);

                string label = month.ToString(
                    "MMM yyyy",
                    culture
                );

                monthlySales.Add(new MonthlySaleViewModel
                {
                    Label = culture.TextInfo.ToTitleCase(label),
                    Total = databaseResult?.Total ?? 0
                });
            }

            return monthlySales;
        }

        public async Task<List<OrderStatusSummaryViewModel>>
            GetOrdersByStatus()
        {
            var databaseResults = await dbContext.Orders
                .AsNoTracking()
                .GroupBy(order => order.Status)
                .Select(group => new
                {
                    Status = group.Key,
                    Quantity = group.Count()
                })
                .OrderBy(result => result.Status)
                .ToListAsync();

            return databaseResults
                .Select(result => new OrderStatusSummaryViewModel
                {
                    Status = string.IsNullOrWhiteSpace(result.Status.ToString())
                        ? "Sin estado"
                        : result.Status.ToString(),

                    Quantity = result.Quantity
                })
                .ToList();
        }
    }

    public class MonthlySaleViewModel
    {
        public string Label { get; set; } = string.Empty;

        public decimal Total { get; set; }
    }

    public class OrderStatusSummaryViewModel
    {
        public string Status { get; set; } = string.Empty;

        public int Quantity { get; set; }
    }
}
