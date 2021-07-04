using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;
        private readonly List<string> _translations;

        public ExpressionToFtsRequestTranslator()
        {
            _resultStringBuilder = new StringBuilder();
            _translations = new List<string>();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);

            return _resultStringBuilder.ToString();
        }

        public IEnumerable<string> GetTranslations(Expression exp)
        {
            Visit(exp);

            return _translations;
        }
        
        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            switch (node.Method.Name)
            {
                case "Where" when node.Method.DeclaringType == typeof(Queryable):
                    var predicate = node.Arguments[1];
                    Visit(predicate);
                    return node;
                
                case "Equals":
                    TryVisitMember(node);
                    Visit(node.Arguments[0]);
                    _resultStringBuilder.Append(')');
                    
                    return node;
                
                case "StartsWith":
                    TryVisitMember(node);
                    Visit(node.Arguments[0]);
                    _resultStringBuilder.Append("*)");
                    
                    return node;
                
                case "EndsWith":
                    TryVisitMemberWithArguments(node);
                    _resultStringBuilder.Append(')');
                    return node;
                
                case "Contains":
                    TryVisitMemberWithArguments(node);
                    _resultStringBuilder.Append("*)");
                    return node;

                default: return base.VisitMethodCall(node);
            }
        }
        
        private void TryVisitMemberWithArguments(MethodCallExpression node)
        {
            TryVisitMember(node);
            _resultStringBuilder.Append('*');
            Visit(node.Arguments[0]);
        }
        
        private void TryVisitMember(MethodCallExpression node)
        {
            if (!(node.Object is MemberExpression member))
            {
                throw new NotSupportedException($"Operation '{node.Method.Name}' is not supported.");
            }

            Visit(member);
            _resultStringBuilder.Append('(');
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    node = ProcessBinaryEquals(node);
                    return node;
                
                case ExpressionType.AndAlso:
                    ProcessAndAlsoOperand(node.Left);
                    ProcessAndAlsoOperand(node.Right);
                    return node;
                
                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported.");

            }
        }
        
        private void ProcessAndAlsoOperand(Expression expression)
        {
            Visit(expression);
            _translations.Add(_resultStringBuilder.ToString());
            _resultStringBuilder.Clear();
        }
        
        private BinaryExpression ProcessBinaryEquals(BinaryExpression node)
        {
            if (node.Left.NodeType == node.Right.NodeType)
            {
                throw new NotSupportedException($"Operation '{node.NodeType}' is not supported.");
            }

            if (node.Left.NodeType != ExpressionType.MemberAccess)
            {
                var expr = Expression.MakeBinary(node.NodeType, node.Right, node.Left);
                VisitBinary(expr);
                return node;
            }

            Visit(node.Left);
            _resultStringBuilder.Append('(');
            Visit(node.Right);
            _resultStringBuilder.Append(')');

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append(node.Member.Name).Append(':');

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultStringBuilder.Append(node.Value);

            return node;
        }

        #endregion
    }
}
