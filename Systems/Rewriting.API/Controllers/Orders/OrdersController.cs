using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rewriting.Common.Validator;
using Rewriting.Services.Orders;
using System.Runtime.InteropServices;

namespace Rewriting.API.Controllers.Orders
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IMapper _mapper;
        private IModelValidator<AddOrderRequest> _validator;
        private IOrderService _orderService;

        public OrdersController(IMapper mapper, IModelValidator<AddOrderRequest> validator, IOrderService orderService)
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
        /// Get delailed information about an order with specified Uid including offers and results
        /// </summary>
        /// <param name="orderGuid">Uid of the target offer</param>
        /// <returns>OrderDetailsResponse</returns>
        [HttpGet]
        public async Task<OrderDetailsResponse> GetOrderDetails(Guid orderGuid)
        {
            var orderModel = await _orderService.GetOrderDetails(orderGuid);
            return _mapper.Map<OrderDetailsResponse>(orderModel);
        }

        /// <summary>
        /// Add new order
        /// </summary>
        /// <param name="request">Add order request model</param>
        /// <returns>OrderDetailsResponse with information about created order</returns>
        [HttpPost]
        public async Task<OrderDetailsResponse> AddOrder(AddOrderRequest request)
        {
            _validator.Check(request);
            var orderModel =  _mapper.Map<AddOrderModel>(request);
            var result = _orderService.AddOrder(orderModel);
            return _mapper.Map<OrderDetailsResponse>(result);
        }

        /// <summary>
        /// Delete order
        /// </summary>
        /// <param name="orderGuid">Uid of target order</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteOrder(Guid orderGuid)
        {
            await _orderService.DeleteOrder(orderGuid);
        }
    }
}
