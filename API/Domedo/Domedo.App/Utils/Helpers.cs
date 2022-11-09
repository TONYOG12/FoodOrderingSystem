using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.Utils
{
    public static class Helpers
    {
        public static Paginateable<T> GetPaginatedData<T>(IPagingList model) where T : class
        {
            return new()
            {
                Data = model as T,
                PageCount = model.PageCount,
                PageIndex = model.PageIndex,
                TotalRecordCount = model.TotalRecordCount,
                NumberOfPagesToShow = model.NumberOfPagesToShow,
                StartPageIndex = model.StartPageIndex,
                StopPageIndex = model.StopPageIndex
            };
        }

    }
}
