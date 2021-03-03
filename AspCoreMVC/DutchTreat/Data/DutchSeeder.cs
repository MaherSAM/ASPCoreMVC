using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _ctx;
        private readonly IHostingEnvironment _hosting;

        public UserManager<StoreUser> _userManager { get; }

        public DutchSeeder(DutchContext ctx,IHostingEnvironment hosting, UserManager<StoreUser> userManager )
        {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();

            StoreUser user = await _userManager.FindByEmailAsync("mahersammari@gmail.com");
            if(user==null)
            {
                user = new StoreUser()
                {
                    FirstName = "Maher",
                    LastName = "Maher",
                    Email = "mahersammari@gmail.com",
                    UserName = "mahersammari@gmail.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if(result!=IdentityResult.Success)
                {
                    throw new InvalidOperationException("Cold not create new user in seeder");
                }
            }
            if(!_ctx.Products.Any())
            {
                // Need to create sample data
                var filePath = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
                var jsonData = File.ReadAllText(filePath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(jsonData);
                _ctx.AddRange(products);

                var order = _ctx.Orders.Where(o => o.Id == 1).FirstOrDefault();
                if(order!=null)
                {
                    order.User = user;
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.FirstOrDefault(),
                            Quantity = 5,
                            UnitPrice = products.FirstOrDefault().Price
                        }
                    };
                }

                _ctx.SaveChanges();
            }
           
        }
    }
}
