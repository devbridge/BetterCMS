using System.Linq;

using Devbridge.Platform.Core.DataAccess;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Tests.DataAccess
{
    [TestFixture]
    public class PredicateBuilderTests : TestBase
    {
        private static Fruit[] Values =
        {
            new Fruit {Name ="orange", IsCitrus = true},
            new Fruit {Name ="lemon", IsCitrus = true},
            new Fruit {Name ="pear", IsCitrus = false},
            new Fruit {Name ="apple", IsCitrus = false}
        };

        [Test]
        public void Should_Filter_False_Or_Correctly()
        {
            var predicateBuilder = PredicateBuilder.False<Fruit>();
            predicateBuilder = predicateBuilder.Or(p => p.Name == "orange");
            predicateBuilder = predicateBuilder.Or(p => p.Name == "apple");

            var result = Values.AsQueryable().Where(predicateBuilder).ToList();
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 2);
            Assert.Contains(Values.First(v => v.Name == "orange"), result);
            Assert.Contains(Values.First(v => v.Name == "apple"), result);
        }
        
        [Test]
        public void Should_Filter_True_And_Correctly()
        {
            var predicateBuilder = PredicateBuilder.True<Fruit>();
            predicateBuilder = predicateBuilder.And(p => p.Name == "lemon");
            predicateBuilder = predicateBuilder.And(p => p.IsCitrus);

            var result = Values.AsQueryable().Where(predicateBuilder).ToList();
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 1);
            Assert.Contains(Values.First(v => v.Name == "lemon"), result);
        }

        private class Fruit
        {
            public string Name { get; set; }
            
            public bool IsCitrus { get; set; }
        }
    }
}
