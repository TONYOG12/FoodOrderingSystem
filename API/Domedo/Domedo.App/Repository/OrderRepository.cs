using AutoMapper;
using Domedo.App.Extensions;
using Domedo.App.IRepositroy;
using Domedo.App.Utils;
using Domedo.Domain.Context;
using Domedo.Domain.Entities;
using Domedo.Domain.Models;
using Domedo.Domain.Requests.Order;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DomedoDbContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(DomedoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderDto> CreateOrder(CreateOrderRequest request)
        {
            var orderItems = _mapper.Map<List<OrderItem>>(request.OrderItems);

            var result = orderItems.Aggregate(new OrderStats(),
                                                 (item, order) => item.Accumulate(order));

            var order = new Order
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Latitude = request.Latitude,
                Longitude  = request.Longitude,
                Comments = request.Comments,
                OrderItems = orderItems,
                TotalPrice = result.TotalPrice,
                CreatedAt = DateTime.Now
            };

           


            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return await GetOrder(order.Id);
        }

        public async Task<OrderDto> GetOrder(Guid id)
        {
            return _mapper.Map<OrderDto>(await _context.Orders
                            .FirstOrDefaultAsync(item => item.Id == id));
        }

        public async Task<Paginateable<IEnumerable<OrderDto>>> GetOrders(int page, int pageSize,
            string searchQuery, Guid userId, bool withCompleted = false)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);


            var query = await _context.Orders
                .CustomIgnoreQueryFilters(withCompleted)
                .OrderByDescending(item => item.CreatedAt)
                .ToListAsync();

            var orders = _mapper.Map<IEnumerable<OrderDto>>(query).ToList();

            orders.ForEach(item => item.OrderItems = GetOrderItemsByOrderId(item.Id));

            if (!string.IsNullOrEmpty(searchQuery))
            {
                orders = orders.Where(
                        q => q.FirstName.ToLower().Contains(searchQuery.ToLower())
                            ||
                            q.LastName.ToLower().Contains(searchQuery.ToLower())
                            ||
                            q.PhoneNumber.ToLower().Contains(searchQuery.ToLower())
                    )
                    .ToList();
            }

            var model = PagingList.Create(orders.Select(item => _mapper.Map<OrderDto>(item)), pageSize, page);
            return Helpers.GetPaginatedData<IEnumerable<OrderDto>>(model);
        }

        private List<OrderItemDto> GetOrderItemsByOrderId(Guid id)
        {
            var query = _context.OrderItems
                 .Where(item => item.OrderId == id)
                 .OrderByDescending(item => item.CreatedAt)
                 .ToList();

            return _mapper.Map<List<OrderItemDto>>(query);
        }

        public async Task CompleteOrder(Guid id)
        {
            var order = await _context.Orders
                                .FirstOrDefaultAsync(item => item.Id == id);

            if (order == null) return;
            if(!order.Completed)
            {
                order.Completed = true;
            }
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

        }
        public async Task UpdateOrder(UpdateOrderRequest request, Guid id)
        {

            var order = await _context.Orders.FirstOrDefaultAsync(item => item.Id == id);
            if (order != null)
            {
                var orderItems = await _context.OrderItems
                                    .Where(item => item.OrderId == order.Id)
                .ToListAsync();

                _context.OrderItems.RemoveRange(orderItems);

                order.OrderItems = _mapper.Map<List<OrderItem>>(request.OrderItems);
                order.UpdatedAt = DateTime.Now;

                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
