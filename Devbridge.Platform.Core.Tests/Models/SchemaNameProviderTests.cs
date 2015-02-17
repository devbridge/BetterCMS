using Devbridge.Platform.Core.Models;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Tests.Models
{
    [TestFixture]
    public class SchemaNameProviderTests : TestBase
    {
        [Test]
        public void Should_Provide_Correct_Schema_Name()
        {
            var origPattern = SchemaNameProvider.SchemaNamePattern;

            try
            {
                SchemaNameProvider.SchemaNamePattern = "test_{0}_test";

                Assert.AreEqual(SchemaNameProvider.GetSchemaName("Module"), "test_Module_test");
            }
            finally
            {
                SchemaNameProvider.SchemaNamePattern = origPattern;
            }
        }
    }
}
