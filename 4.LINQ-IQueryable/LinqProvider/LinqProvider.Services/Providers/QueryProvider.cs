using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LinqProvider.Services.Helpers;

namespace LinqProvider.Services.Providers
{
    public abstract class QueryProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
            //ex. was found in https://docs.microsoft.com/en-us/archive/blogs/mattwar/linq-building-an-iqueryable-provider-part-i
            var elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(QuerySet<>)
                    .MakeGenericType(elementType), new object[] { this, expression });
            }

            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new QuerySet<TElement>(this, expression);
        }

        public TResult Execute<TResult>(Expression expression) => (TResult)Execute(expression);

        public abstract string GetQueryText(Expression expression);

        public abstract object Execute(Expression expression);
    }
}