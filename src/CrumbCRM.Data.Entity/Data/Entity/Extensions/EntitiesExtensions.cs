using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Data.Entity.Extensions
{
    public static class EntitiesExtensions
    {
        public static IQueryable<T> ToPagedQueryable<T>(this IQueryable<T> items, PagingSettings paging)
        {
            return items.Skip((paging.PageIndex - 1) * paging.PageCount).Take(paging.PageCount);
        }

        public static List<T> ToPagedList<T>(this IQueryable<T> items, PagingSettings paging)
        {
            return ToPagedQueryable(items, paging).ToList();
        }

        public static List<T> ToPagedList<T>(this List<T> items, PagingSettings paging)
        {
            return items.Skip((paging.PageIndex - 1) * paging.PageCount).Take(paging.PageCount).ToList();
        }
    }
}
