using Application.DTOs.Order;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Application.Repositories
{
    public interface IOrderRepository
    {
        Task<IQueryable<Order>> GetAllOrdersWithDetailsAsync();
        Task<Order> GetOrderWithDetailsByIdAsync(Guid id);
    }

}
