using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqProvider.Services
{
    public class QuerySet<T> : IQueryable<T>
    {
        private readonly QueryProvider _provider;

        public QuerySet(QueryProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            Expression = Expression.Constant(this);
        }
        
        public QuerySet(QueryProvider provider, Expression expression)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));

            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException(nameof(expression));
            }
        }

        public IEnumerator<T> GetEnumerator() => _provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _provider.Execute<IEnumerable>(Expression).GetEnumerator();

        public override string ToString() => _provider.GetQueryText(Expression);
        
        public Type ElementType => typeof(T);
        public Expression Expression { get; }
        public IQueryProvider Provider => _provider;
    }
}
