using System.Linq;
using System.Linq.Expressions;

namespace Zarasa.Editorial.Api.Common.Extensions
{
    public static class IQueryableExtensions 
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderBy(IQueryableExtensions.ToLambda<T>(propertyName));
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, string direction)
        {
            if(direction == "desc") 
            {
                return OrderByDescending(source, propertyName);
            }
            else 
            {
                return OrderBy(source, propertyName);
            }
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderByDescending(IQueryableExtensions.ToLambda<T>(propertyName));
        }

        private static Expression<System.Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<System.Func<T, object>>(propAsObject, parameter);            
        }
    }
}