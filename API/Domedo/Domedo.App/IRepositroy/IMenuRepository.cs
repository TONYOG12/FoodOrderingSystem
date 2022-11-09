using Domedo.App.Utils;
using Domedo.Domain.Models;
using Domedo.Domain.Requests.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.IRepositroy
{
    public interface IMenuRepository
    {
        public Task<MenuDto> CreateMenu(CreateMenuRequest request, Guid userId);

        public Task<MenuDto> GetMenu(Guid menuId);

        public Task<Paginateable<IEnumerable<MenuDto>>> GetMenus(int page, int pageSize, string searchQuery, Guid userId, bool withDeleted = false);

        //public IEnumerable<MealDto> GetMealsByMenuId(Guid menuId);

        public Task<Paginateable<IEnumerable<MealMenuDto>>> GetMealMenus(int page, int pageSize,
         string searchQuery, bool withDeleted = false);
        public Task UpdateMenu(UpdateMenuRequest request, Guid id, Guid userId);

        public Task DeleteMenu(Guid id, Guid userId);

    }
}
