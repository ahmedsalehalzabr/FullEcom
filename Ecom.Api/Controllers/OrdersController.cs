﻿using Ecom.Core.Dto;
using Ecom.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("create-order")]
        public async Task<ActionResult> create(OrderDto orderDto)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var order = await _orderService.CreateOrdersAsync(orderDto, email);
            return Ok(order);
        }
    }
}
