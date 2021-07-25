using System;
using System.Linq;
using NUnit.Framework;
using System.Data.SqlClient;
using LinqProvider.Services.Data;

namespace LinqProvider.Tests
{
    public class LinqProviderTests
    {
        private string ConnectionString;

        [SetUp]
        public void SetUp()
        {
            ConnectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=Northwind;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        [Test]
        public void ProductsSet_Where_Test()
        {
            var expectedNames = new[] { "Thüringer Rostbratwurst", "Côte de Blaye" };

            using var sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();

            var db = new Northwind(sqlConnection);
            var query = db.Products.Where(p => p.UnitPrice > 100);

            Console.WriteLine($"Query:\n{query}\n");

            var products = query.ToArray();

            Assert.NotNull(products);
            Assert.AreEqual(2, products.Length);
            
            foreach (var product in products)
            {
                Assert.IsTrue(expectedNames.Contains(product.ProductName));
            }
        }
        
        [Test]
        public void ProductsSet_Where_And_Test()
        {
            var expectedIds = new[] { 9, 18, 20, 38, 51, 59 };

            using var sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();

            var db = new Northwind(sqlConnection);
            var query = db.Products.Where(p => p.UnitPrice > 50 && p.UnitsInStock > 1);

            Console.WriteLine($"Query:\n{query}\n");

            var products = query.ToArray();

            Assert.NotNull(products);
            Assert.AreEqual(6, products.Length);
            
            foreach (var product in products)
            {
                Assert.IsTrue(expectedIds.Contains(product.ProductID));
            }
        }
    }
}
