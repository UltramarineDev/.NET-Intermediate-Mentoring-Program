using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        [TestMethod]
        public void Map()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();

            var source = new Foo
            {
                Name = "TestName",
                Surname = "TestSurname",
                Age = 10,
                Note = "note"
            };

            var result = mapper.Map(source);

            Assert.AreEqual(source.Name, result.Name);
            Assert.AreEqual(source.Age, result.Age);
        }
    }
}
