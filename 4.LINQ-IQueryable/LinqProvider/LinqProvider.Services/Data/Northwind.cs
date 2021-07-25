using System.Data.Common;
using LinqProvider.Services.Data.Entities;
using LinqProvider.Services.Providers;

namespace LinqProvider.Services.Data
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
