using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using LinqProvider.Services.Entities;

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

            var data = new DataTable();
            data.Load(command.ExecuteReader());

            Type elementType = TypeSystem.GetElementType(expression.Type);
            
            List<Products> CustomObjectList;

            using DataTable dtCustomer = data;
            
            CustomObjectList = dtCustomer.ToList<Products>();
            return CustomObjectList;
        }

        private string Translate(Expression expression) => new QueryTranslator().Translate(expression);
    }
}
