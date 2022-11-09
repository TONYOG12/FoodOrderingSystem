using AutoMapper;
using Domedo.App.Extensions;
using Domedo.App.IRepositroy;
using Domedo.App.Utils;
using Domedo.Domain.Context;
using Domedo.Domain.Entities;
using Domedo.Domain.Models;
using Domedo.Domain.Requests.Menu;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.Repository
{
    public class MenuRepository : IMenuRepository
    {
        private readonly DomedoDbContext _context;
        private readonly IMapper _mapper;

        public MenuRepository(DomedoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MenuDto> CreateMenu(CreateMenuRequest request, Guid userId)
        {
            var menu = new Menu
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.Now,
                CreatedBy = userId
            };

            await _context.Menus.AddAsync(menu);
            await _context.SaveChangesAsync();

            return await GetMenu(menu.Id);
        }

        public async Task<MenuDto> GetMenu(Guid menuId)
        {
            return _mapper.Map<MenuDto>(await _context.Menus
               .FirstOrDefaultAsync(item => item.Id == menuId));
        }

        public async Task<Paginateable<IEnumerable<MenuDto>>> GetMenus(int page, int pageSize, string searchQuery, Guid userId, bool withDeleted = false)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);


            var query = await _context.Menus
                .CustomIgnoreQueryFilters(withDeleted)
                .OrderByDescending(item => item.CreatedAt)
                .ToListAsync();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(
                        q => q.Name.ToLower().Contains(searchQuery.ToLower())
                            ||
                            q.Description.ToLower().Contains(searchQuery.ToLower())
                    )
                    .ToList();
            }

            var model = PagingList.Create(query.Select(item => _mapper.Map<MenuDto>(item)), pageSize, page);
            return Helpers.GetPaginatedData<IEnumerable<MenuDto>>(model);
        }
        public IEnumerable<MealDto> GetMealsByMenuId(Guid menuId)
        {
            var query = _context.Meals
                .Where(item => item.MenuId == menuId)
                .OrderByDescending(item => item.CreatedAt)
                .ToList();

            return _mapper.Map<IEnumerable<MealDto>>(query);

        }
        public async Task<Paginateable<IEnumerable<MealMenuDto>>> GetMealMenus(int page, int pageSize,
          string searchQuery, bool withDeleted = false)
        {
            var menus = await _context.Menus
                .CustomIgnoreQueryFilters(withDeleted)
                .OrderByDescending(item => item.CreatedAt)
                .ToListAsync();

            var mealMenus = (_mapper.Map<IEnumerable<MealMenuDto>>(menus)).ToList();

            mealMenus.ForEach(item => item.Meals = GetMealsByMenuId(item.Id));

            if (!string.IsNullOrEmpty(searchQuery))
            {
                mealMenus = mealMenus.Where(
                        q => q.Name.ToLower().Contains(searchQuery.ToLower())

                    )
                    .ToList();
            }

            var model = PagingList.Create(mealMenus.Select(item => _mapper.Map<MealMenuDto>(item)), pageSize, page);
            return Helpers.GetPaginatedData<IEnumerable<MealMenuDto>>(model);

        }


        public async Task UpdateMenu(UpdateMenuRequest request, Guid id, Guid userId)
        {
            var menu = await _context.Menus.FirstOrDefaultAsync(item => item.Id == id);
            if (menu != null)
            {
                menu.Name = request.Name;
                menu.Description = request.Description;
                menu.LastUpdatedBy = userId;
                menu.UpdatedAt = DateTime.Now;

                _context.Menus.Update(menu);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteMenu(Guid id, Guid userId)
        {
            var menu = await _context.Menus.FirstOrDefaultAsync(item => item.Id == id);
            if (menu != null)
            {
                menu.DeletedAt = DateTime.Now;
                menu.LastDeletedBy = userId;
                _context.Menus.Update(menu);
                await _context.SaveChangesAsync();
            }
        }
    }
}

