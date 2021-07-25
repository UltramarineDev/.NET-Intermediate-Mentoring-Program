using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using LinqProvider.Services.Helpers;

namespace LinqProvider.Services.Providers
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

            using var dataTable = new DataTable();
            dataTable.Load(command.ExecuteReader());

            var objectType = TypeSystem.GetElementType(expression.Type);
            var objectList = dataTable.ToList(objectType);
            return objectList;
        }

        private static string Translate(Expression expression) => new QueryTranslator().Translate(expression);
    }
}
