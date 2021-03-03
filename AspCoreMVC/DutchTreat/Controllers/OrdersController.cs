using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]

    public class OrdersController : Controller
    {
        private readonly IDutchRepository _repository;

        public ILogger<OrdersController> _logger { get; }

        private readonly IMapper _mapper;

        public UserManager<StoreUser> _userManager { get; }

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger, IMapper mapper,UserManager<StoreUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }
        [HttpGet]
      
        public ActionResult<IEnumerable<Order>> Get(bool includeItems=true)
        {
            try
            {
                var username = User.Identity.Name;


                return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(_repository.GetAllOrdersByUser(username,includeItems)));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all oreders {0}", ex);
                return BadRequest("Failde to get all orders");
            }



        }
        [HttpGet("{id:int}")]
        public ActionResult<Order> Get(int id)
        {
            try
            {
                var username = User.Identity.Name;

                var order = _repository.GetOrderById(username, id);
                if (order != null)
                {
                    return Ok(_mapper.Map<Order, OrderViewModel>(order));
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get oreder {0}", ex);
                return BadRequest("Failde to get order");
            }



        }
        [HttpPost]
        public async  Task<IActionResult> Post([FromBody] OrderViewModel model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                  
                    var newOrder = _mapper.Map<OrderViewModel, Order>(model);

                    if (newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

                    newOrder.User = currentUser;

                    _repository.AddEntity(newOrder);

                    if (_repository.SaveAll())
                    {
                    
                  
                        return Created($"api/orders/{newOrder.Id}",_mapper.Map<Order,OrderViewModel>(newOrder));
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }


            }
            catch (Exception ex)
            {

                _logger.LogError($"Failde to add new order {0}", ex);

            }

            return BadRequest("Failde to add new order");

        }
    }
}
