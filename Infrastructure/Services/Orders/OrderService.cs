using Application.Abstraction;
using Application.DTOs.Order;
using Application.Repositories;
using AutoMapper;
using Domain.Models;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly IProductRepository productRepository;
        private readonly IUserRepository userRepository;
        private readonly IRetailingOrderingSystemDbContext context;
        public OrderService(IOrderRepository orderRepository,
            IMapper mapper,
            IProductRepository productRepository,
            IUserRepository userRepository,
            IRetailingOrderingSystemDbContext context)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.productRepository = productRepository;
            this.userRepository = userRepository;
            this.context = context;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto orderDto)
        {
            var user = await userRepository.GetByIdAsync(orderDto.UserId);
            if (user == null) throw new KeyNotFoundException("User not found.");

            var order = new Order
            {
                UserId = orderDto.UserId,
                PaymentStatus = "Pending",
                OrderDate = DateTime.Now,
                OrderDetails = new List<OrderDetail>()
            };

            foreach (var itemDto in orderDto.OrderDetails)
            {
                var product = await productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null) throw new KeyNotFoundException($"Product with ID {itemDto.ProductId} not found.");
                if (product.Stock < itemDto.Quantity) throw new Exception($"Not enough stock for product {product.Name}.");

                product.Stock -= itemDto.Quantity;

                var orderItem = new OrderDetail
                {
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    Price = product.Price
                };

                order.OrderDetails.Add(orderItem);
                order.TotalAmount += product.Price * itemDto.Quantity;
            }

            if (order.TotalAmount <= 0) throw new Exception("Order total amount must be greater than 0.");

            await context.Orders.AddAsync(order);

            var result = await context.SaveChangesAsync();

            var createdOrder = mapper.Map<OrderDto>(order);
            return result > 0 ? createdOrder : null;
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            var order = await context.Orders.FindAsync(id);
            if (order is not null)
                context.Orders.Remove(order);

            var result = await context.SaveChangesAsync();

            return result > 0 ? true : false;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await orderRepository.GetAllOrdersWithDetailsAsync();
            return mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderByIdAsync(Guid id)
        {
            var order = await orderRepository.GetOrderWithDetailsByIdAsync(id);
            if (order is null) throw new KeyNotFoundException("Order not found.");

            return mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> UpdateOrderStatusAsync(Guid id, string status)
        {
            var existingOrder = await context.Orders.FindAsync(id);

            if (existingOrder is not null)
            {
                existingOrder.PaymentStatus = status;
                context.Orders.Update(existingOrder);
            }

            var result = await context.SaveChangesAsync();

            return result > 0 ? mapper.Map<OrderDto>(existingOrder) : null;
        }
    }
}
