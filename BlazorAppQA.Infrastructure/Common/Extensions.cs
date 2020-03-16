using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;

namespace BlazorAppQA.Infrastructure.Common
{
    public static class Extensions
    {
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int page, int pageSize)
        {
            if (page <= 0)
            {
                page = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 5;
            }

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public static dynamic ToExpando(this object obj)
        {
            var result = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<ExpandoObject>(result);
        }
    }
}
