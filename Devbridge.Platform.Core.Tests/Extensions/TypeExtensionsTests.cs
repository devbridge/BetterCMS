using Devbridge.Platform.Core.Extensions;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Tests.Extensions
{
    [TestFixture]
    public class TypeExtensionsTests : TestBase
    {
        [Test]
        public void Should_Check_GenericType_Correctly()
        {
            var instance = new TestClass();
            var type = instance.GetType();

            Assert.IsTrue(type.IsAssignableToGenericType(typeof(ITestInterface1<>)));
            Assert.IsTrue(type.IsAssignableToGenericType(typeof(ITestInterface2<,>)));
            Assert.IsTrue(type.IsAssignableToGenericType(typeof(AbstractTestBase<>)));
            Assert.IsTrue(type.IsAssignableToGenericType(typeof(AbstractTestBase<>)));
        }
        
        [Test]
        public void Should_Check_NonGenericType_Correctly()
        {
            var instance = new TestClass2();
            var type = instance.GetType();

            Assert.IsFalse(type.IsAssignableToGenericType(typeof(ITestInterface1<>)));
            Assert.IsFalse(type.IsAssignableToGenericType(typeof(ITestInterface2<,>)));
            Assert.IsFalse(type.IsAssignableToGenericType(typeof(AbstractTestBase<>)));
            Assert.IsFalse(type.IsAssignableToGenericType(typeof(AbstractTestBase<>)));
        }

        public interface ITestInterface1<T>
        {
        }
        
        public interface ITestInterface2<T, T1> : ITestInterface1<T>
        {
        }

        public abstract class AbstractTestBase<T> : ITestInterface2<T, int>
        {
        }

        public class TestClass : AbstractTestBase<TestClass2>
        {
        }

        public class TestClass2
        {
        }
    }
}
