﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rewriting.Common.Exceptions;
using Rewriting.Common.Helpers;
using Rewriting.Common.Security;
using Rewriting.Common.Validator;
using Rewriting.Context.Entities;
using Rewriting.Services.Orders;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace Rewriting.API.Controllers.Orders
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IMapper _mapper;
        private IModelValidator<AddOrderRequest> _validator;
        private IOrderService _orderService;

        public OrdersController(
            IMapper mapper, 
            IModelValidator<AddOrderRequest> validator, 
            IOrderService orderService, 
            UserManager<UserIdentity> userManager)
        {
            _mapper = mapper;
            _validator = validator;
            _orderService = orderService;
        }

        /// <summary>
        /// Get all the orders available for offers
        /// </summary>
        /// <returns>The List of OrderResponse representing new orders</returns>
        [HttpGet]
        public async Task<List<OrderResponse>> GetNewOrders()
        {
            var orderModels = await _orderService.GetNewOrders();
            return _mapper.Map<List<OrderResponse>>(orderModels);
        }

        /// <summary>
        /// Get all the orders published by calling user
        /// </summary>
        /// <returns>The List of OrderResponse representing user's orders</returns>
        /// <exception cref="ProcessException"></exception>
        [HttpGet]
        [Authorize(Policy = AppScopes.OrdersRead)]
        public async Task<List<OrderResponse>> GetUserOrders()
        {
            var userUid = User.GetUid();
            
            var orderModels = await _orderService.GetOrdersByUser(userUid);
            return _mapper.Map<List<OrderResponse>>(orderModels);
        }

        /// <summary>
        /// Get delailed information about an order with specified Uid including offers and results
        /// </summary>
        /// <param name="orderUid">Uid of the target offer</param>
        /// <returns>OrderDetailsResponse</returns>
        [HttpGet]
        [Authorize(Policy = AppScopes.OrdersEdit)]
        public async Task<OrderDetailsResponse> GetOrderDetails(Guid orderUid)
        {
            var orderModel = await _orderService.GetOrderDetails(orderUid);
            return _mapper.Map<OrderDetailsResponse>(orderModel);
        }

        /// <summary>
        /// Add new order
        /// </summary>
        /// <param name="request">Add order request model</param>
        /// <returns>OrderDetailsResponse with information about created order</returns>
        [HttpPost]
        [Authorize(Policy = AppScopes.OffersWrite)]
        public async Task<OrderDetailsResponse> AddOrder(AddOrderRequest request)
        {
            _validator.Check(request);

            var orderModel =  _mapper.Map<AddOrderModel>(request);
            orderModel.ClientUid = User.GetUid();

            var result = await _orderService.AddOrder(orderModel);

            return _mapper.Map<OrderDetailsResponse>(result);
        }

        /// <summary>
        /// Cancel order
        /// </summary>
        /// <param name="orderUid">Uid of target order</param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> CancelOrder(Guid orderUid)
        {
            var cancelOrderModel = new CancelOrderModel
            {
                OrderUid = orderUid,
                Issuer = User,
            };

            await _orderService.CancelOrder(cancelOrderModel);

            return Ok();
        }

        /// <summary>
        /// Delete order
        /// </summary>
        /// <param name="orderUid">Uid of target order</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(Guid orderUid)
        {
            await _orderService.DeleteOrder(orderUid);
            return Ok();
        }
    }
}