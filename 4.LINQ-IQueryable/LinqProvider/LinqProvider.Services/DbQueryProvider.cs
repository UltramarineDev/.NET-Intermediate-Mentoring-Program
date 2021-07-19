using System;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqProvider.Services
{
    public class DbQueryProvider : QueryProvider
    {
        private readonly DbConnection _connection;
        
        public DbQueryProvider(DbConnection connection)
        {
            _connection = connection;
        }

        public override string GetQueryText(Expression expression) => Translate(expression);

        public override object Execute(Expression expression)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = Translate(expression);

            var reader = command.ExecuteReader();

            //Type elementType = TypeSystem.GetElementType(expression.Type);

            //// return command.ExecuteNonQuery();
            //return Activator.CreateInstance(

            //    typeof(ObjectReader<>).MakeGenericType(elementType),

            //    BindingFlags.Instance | BindingFlags.NonPublic, null,

            //    new object[] { reader },

            //    null);
        }

        private string Translate(Expression expression) => new QueryTranslator().Translate(expression);
    }
}
