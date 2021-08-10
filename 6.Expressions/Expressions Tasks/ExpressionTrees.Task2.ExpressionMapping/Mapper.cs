using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class Mapper<TSource, TDestination>
    {
        private readonly Type _sourceType;
        private readonly Type _destinationType;

        public Mapper()
        {
            _sourceType = typeof(TSource);
            _destinationType = typeof(TDestination);
        }

        public TDestination Map(TSource source)
        {
            var parameter = Expression.Parameter(typeof(TSource), "source");

            var bodyExpression = BuildConverter(parameter);
            var mappingFunc = Expression.Lambda<Func<TSource, TDestination>>(bodyExpression, parameter).Compile();

            return mappingFunc(source);
        }

        private BlockExpression BuildConverter(ParameterExpression parameter)
        {
            var sourceInstance = Expression.Variable(_sourceType, "typedSource");
            var destinationInstance = Expression.Variable(_destinationType, "destinationInstance");

            var expressions = new List<Expression>
            {
                Expression.Assign(sourceInstance, Expression.Convert(parameter, _sourceType)),
                Expression.Assign(destinationInstance, Expression.New(typeof(TDestination)))
            };

            AssignPropertyValues(sourceInstance, destinationInstance, expressions);

            expressions.Add(destinationInstance);

            return Expression.Block(new[] { sourceInstance, destinationInstance }, expressions);
        }

        private void AssignPropertyValues(ParameterExpression sourceInstance, ParameterExpression destinationInstance, List<Expression> expressions)
        {
            var sourceProperties = _sourceType.GetProperties();
            var destinationProperties = _destinationType.GetProperties()
                .ToDictionary(p => new { p.Name, p.PropertyType });

            foreach (var sourceProperty in sourceProperties)
            {
                if (!destinationProperties.TryGetValue( new { sourceProperty.Name, sourceProperty.PropertyType}, out var outProperty))
                {
                    continue;
                }

                var sourceValue = Expression.Property(sourceInstance, sourceProperty);
                var outValue = Expression.Property(destinationInstance, outProperty);

                expressions.Add(Expression.Assign(outValue, sourceValue));
            }
        }
    }
}
