using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LinqProvider.Services
{
    public class QueryTranslator : ExpressionVisitor
    {
        private readonly StringBuilder _resultQuery;

        public QueryTranslator()
        {
            _resultQuery = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);

            return _resultQuery.ToString();
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable) && node.Method.Name == "Where")
            {
                _resultQuery.Append("SELECT * FROM ");

                Visit(node.Arguments[0]);
                _resultQuery.Append(" WHERE ");
                //var lambda = (LambdaExpression)(node.Arguments[1]);

                //Visit(lambda.Body);
                Visit(node.Arguments[1]);
                return node;
            }

            throw new NotSupportedException(string.Format($"The method '{node.Method.Name}' is not supported."));
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            _resultQuery.Append('(');
            Visit(node.Left);

            switch (node.NodeType)
            {
                case ExpressionType.And:
                    _resultQuery.Append(" AND ");
                    break;

                case ExpressionType.Or:
                    _resultQuery.Append(" OR");
                    break;

                case ExpressionType.Equal:
                    _resultQuery.Append(" = ");
                    break;

                case ExpressionType.LessThan:
                    _resultQuery.Append(" < ");
                    break;

                case ExpressionType.GreaterThan:
                    _resultQuery.Append(" > ");
                    break;

                default:

                    throw new NotSupportedException(string.Format($"The binary operator '{node.NodeType}' is not supported."));

            }

            Visit(node.Right);
            _resultQuery.Append(")");

            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            var nodeValue = node.Value as IQueryable;
            
            if (node.Value == null)
            {
                _resultQuery.Append("NULL");
                return node;
            }

            switch (Type.GetTypeCode(node.Value.GetType()))
            {
                case TypeCode.Boolean:
                    _resultQuery.Append(((bool)node.Value) ? 1 : 0);
                    break;

                case TypeCode.String:
                    _resultQuery.Append('\'');
                    _resultQuery.Append(node.Value);
                    _resultQuery.Append('\'');
                    break;

                case TypeCode.Object:
                    //[dbo].[Products]
                   // _resultQuery.Append("[dbo].[");
                    _resultQuery.Append(nodeValue.ElementType.Name);
                   // _resultQuery.Append("]");
                    break;
                default:
                    _resultQuery.Append(node.Value);
                    break;
            }

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression != null && node.Expression.NodeType == ExpressionType.Parameter)
            {
                _resultQuery.Append(node.Member.Name);
                return node;
            }

            throw new NotSupportedException(string.Format($"The member '{node.Member.Name}' is not supported."));
        }
    }
}