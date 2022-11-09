using AutoMapper;
using Domedo.App.Extensions;
using Domedo.App.IRepositroy;
using Domedo.App.Utils;
using Domedo.Domain.Context;
using Domedo.Domain.Entities;
using Domedo.Domain.Models;
using Domedo.Domain.Requests.Meal;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.Repository
{
    public class MealRepository : IMealRepository
    {
        private readonly DomedoDbContext _context;
        private readonly IMapper _mapper;

        public MealRepository(DomedoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MealDto> CreateMeal(CreateMealRequest request, Guid userId)
        {
            var meal = new Meal
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                MenuId = request.MenuId,
                CreatedBy = userId,
                CreatedAt = DateTime.Now
            };

            await _context.Meals.AddAsync(meal);
            await _context.SaveChangesAsync();

            return await GetMeal(meal.Id);

        }

        public async Task<MealDto> GetMeal(Guid mealId)
        {
            return _mapper.Map<MealDto>(await _context.Meals
               .FirstOrDefaultAsync(item => item.Id == mealId));
        }

        public async Task<Paginateable<IEnumerable<MealDto>>> GetMealsByMenuId(int page, int pageSize, Guid menuId, bool withDeleted = false)
        {
            var query = await _context.Meals
                .CustomIgnoreQueryFilters(withDeleted)
                .Where(item => item.MenuId == menuId)
                .OrderByDescending(item => item.CreatedAt)
                .ToListAsync();

            var model = PagingList.Create(query.Select(item => _mapper.Map<MealDto>(item)), pageSize, page);

            return Helpers.GetPaginatedData<IEnumerable<MealDto>>(model);
        }

        public async Task<Paginateable<IEnumerable<MealDto>>> GetMeals(int page, int pageSize, string searchQuery, bool withDeleted = false)
        { 
            var query = await _context.Meals
                .CustomIgnoreQueryFilters(withDeleted)
                .OrderByDescending(item => item.CreatedAt)
                .ToListAsync();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(
                        q => q.Name.ToLower().Contains(searchQuery.ToLower())
                             ||
                             q.Price.ToString().ToLower().Contains(searchQuery.ToLower())
                    )
                    .ToList();
            }

            var model = PagingList.Create(query.Select(item => _mapper.Map<MealDto>(item)), pageSize, page);

            return Helpers.GetPaginatedData<IEnumerable<MealDto>>(model);
        }

        public async Task UpdateMeal(UpdateMealRequest request, Guid id, Guid userId)
        {
            var meal = await _context.Meals.FirstOrDefaultAsync(item => item.Id == id);
            if (meal != null)
            {
                meal.Name = request.Name;
                meal.Description = request.Description;
                meal.Price = request.Price;
                meal.MenuId = request.MenuId;
                meal.LastUpdatedBy = userId;
                meal.UpdatedAt = DateTime.Now;

                _context.Meals.Update(meal);
                await _context.SaveChangesAsync();

            }
        }

        public async Task DeleteMeal(Guid id, Guid userId)
        {
            var meal = await _context.Meals.IgnoreQueryFilters().FirstOrDefaultAsync(item => item.Id == id);
            if (meal != null)
            {
                meal.DeletedAt = DateTime.Now;
                meal.LastDeletedBy = userId;
                _context.Meals.Update(meal);
                await _context.SaveChangesAsync();
            }
        }
    }
}
