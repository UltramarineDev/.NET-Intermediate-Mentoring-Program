using System.Data.Common;
using LinqProvider.Services.Entities;

namespace LinqProvider.Services
{
    public class Northwind
    {
        public QuerySet<Products> Products { get; }

        public Northwind(DbConnection connection)
        {
            var provider = new DbQueryProvider(connection);
            Products = new QuerySet<Products>(provider);
        }
    }
}
