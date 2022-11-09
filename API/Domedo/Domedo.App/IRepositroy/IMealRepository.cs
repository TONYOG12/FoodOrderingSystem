using Domedo.App.Utils;
using Domedo.Domain.Models;
using Domedo.Domain.Requests.Meal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.IRepositroy
{
    public interface IMealRepository
    {
        public Task<MealDto> CreateMeal(CreateMealRequest request, Guid userId);
        public Task<MealDto> GetMeal(Guid mealId);
        public Task<Paginateable<IEnumerable<MealDto>>> GetMealsByMenuId(int page, int pageSize, Guid menuId, bool withDeleted = false);
        public Task<Paginateable<IEnumerable<MealDto>>> GetMeals(int page, int pageSize, string searchQuery, bool withDeleted = false);
        public Task UpdateMeal(UpdateMealRequest request, Guid id, Guid userId);
        public Task DeleteMeal(Guid id, Guid userId);


    }
}
