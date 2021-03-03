using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _ctx;

        public ILogger<DutchRepository> _logger { get; }

        public DutchRepository(DutchContext ctx, ILogger<DutchRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            _logger.LogInformation("GetAllProducts was called");
            try
            {
                return _ctx.Products.OrderBy(p => p.Title).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all products: {ex}");
                return null;
            }



        }
        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _ctx.Products.Where(el => el.Category.ToUpper() == category.ToUpper()).OrderBy(el => el.Title);
        }
        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems )
        {
            if(includeItems)
            {
                return _ctx.Orders.Include(o => o.Items).ThenInclude(p => p.Product).ToList();
            }
            return _ctx.Orders.ToList();
        }
        public IEnumerable<Order> GetAllOrdersByUser(string username,bool includeItems)
        {
            if (includeItems)
            {
                return _ctx.Orders.Include(o => o.Items).ThenInclude(p => p.Product).Where(el=>el.User.UserName==username).ToList();
            }
            return _ctx.Orders.Where(el => el.User.UserName == username).ToList();
        }

        public Order GetOrderById(string username,int id)
        {
           return _ctx.Orders.Where(el => el.User.UserName == username).Include(o => o.Items).ThenInclude(p => p.Product).FirstOrDefault(o=>o.Id==id);
        }
        public void AddEntity(object model)
        {
            _ctx.Add(model);
        }
        public void AddOrder(Order order)
        {
            _ctx.Orders.Add(order);
        }
    }
}
