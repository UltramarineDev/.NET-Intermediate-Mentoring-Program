using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        private const int RightOperand = 1;
        private Dictionary<string, int> _parametersToTransformPairs;

        public IncDecExpressionVisitor()
        {
            _parametersToTransformPairs = new Dictionary<string, int>();
        }

        public Expression Translate(Expression exp)
        {
            return VisitAndConvert(exp, "");
        }

        public Expression Translate(Expression exp, Dictionary<string, int> parametersToTransformPairs)
        {
            _parametersToTransformPairs = parametersToTransformPairs;
            return VisitAndConvert(exp, "");
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return Expression.Lambda(Visit(node.Body), node.Parameters);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.Add)
            {
                var (isIncrement, param) = IsIncOrDecExpression(node);
                if (isIncrement)
                {
                    return Expression.Increment(param);
                }
            }

            if (node.NodeType == ExpressionType.Subtract)
            {
                var (isDecrement, param) = IsIncOrDecExpression(node);
                if (isDecrement)
                {
                    return Expression.Decrement(param);
                }
            }

            return base.VisitBinary(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_parametersToTransformPairs.TryGetValue(node.Name, out var valueToReplace))
            {
                return Expression.Constant(valueToReplace);
            }

            return base.VisitParameter(node);
        }

        private (bool, ParameterExpression) IsIncOrDecExpression(BinaryExpression node)
        {
            ParameterExpression param = null;
            ConstantExpression constant = null;

            if (node.Left.NodeType == ExpressionType.Parameter)
            {
                param = (ParameterExpression)node.Left;
            }

            if (node.Right.NodeType == ExpressionType.Constant)
            {
                constant = (ConstantExpression)node.Right;
            }

            if (param != null && constant != null && constant.Type == typeof(int) && (int)constant.Value == RightOperand)
            {
                return (true, param);
            }

            return (false, param);
        }
    }
}
