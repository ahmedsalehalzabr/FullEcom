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
        [HttpGet("get-orders-for-user")]
        public async Task<IActionResult> getorders()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var order = await _orderService.GetAllOrdersForUserAsync(email);
            return Ok(order);
        }
        [HttpGet("get-order-by-id/{id}")]
        public async Task<IActionResult> getOrderById(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var order = await _orderService.GetOrderByIdAsync(id, email);
            return Ok(order);
        }
        [HttpGet("get-delivery")]
        public async Task<IActionResult> GetDelivery()
            => Ok(await _orderService.GetDeliveryMethodAsync());

    }
}
