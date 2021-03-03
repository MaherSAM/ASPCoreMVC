using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    [Route("/api/orders/{orderid}/items")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderItemsController : Controller
    {
        private readonly IDutchRepository _repository;

        public ILogger<OrderItemsController> _logger { get; }

        private readonly IMapper _mapper;

        public OrderItemsController(IDutchRepository repository, ILogger<OrderItemsController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public IActionResult Get(int orderId)
        {
            try
            {
                string username = User.Identity.Name;

                var order = _repository.GetOrderById(username,orderId);
                if(order!=null)
                {
                    return Ok(_mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get order items {0}",ex);
               
            }
            return BadRequest("Failed to get order items");
        }
        [HttpGet("{id}")]
        public IActionResult Get(int orderId,int id)
        {
            try
            {
                string username = User.Identity.Name;
                var order = _repository.GetOrderById(username,orderId);
                if (order != null && order.Items!=null && order.Items.FirstOrDefault(el=>el.Id==id)!=null)
                {
                    return Ok(_mapper.Map<OrderItem, OrderItemViewModel>(order.Items.FirstOrDefault(el => el.Id == id)));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get order item {0}", ex);

            }
            return BadRequest("Failed to get order item");
        }
    }
}
