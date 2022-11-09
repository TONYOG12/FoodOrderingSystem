using Domedo.App.Utils;
using Domedo.Domain.Models;
using Domedo.Domain.Requests.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.IRepositroy
{
    public interface IOrderRepository
    {
        public Task<OrderDto> CreateOrder(CreateOrderRequest request);
        public Task<OrderDto> GetOrder(Guid id);
        public Task<Paginateable<IEnumerable<OrderDto>>> GetOrders(int page, int pageSize, string searchQuery, Guid userId, bool withCompleted = false);
        public Task CompleteOrder(Guid id);
        public Task UpdateOrder(UpdateOrderRequest request, Guid id);

    }
}
