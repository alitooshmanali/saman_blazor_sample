using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamanProject.Application
{
    public static class Extensions
    {
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            if (pageSize == -1 && pageIndex == -1)
                return query;

            if (pageSize == 0)
                pageSize = 100;

            if (pageIndex == 0)
                pageIndex = 1;

            var start = pageSize * (pageIndex - 1);
            query
                .Skip(start)
                .Take(pageSize);

            return query;
        }
    }

}
