using System;
using System.Linq;
using System.Text;

using BetterModules.Core.Models;

using NUnit.Framework;

namespace BetterCms.Test.Module.Common
{
    [TestFixture]
    public class SerializationTest : SerializationTestBase
    {
        [Test]
        public void All_Database_Entities_Should_Be_Serializable()
        {
            Type entityBaseType = typeof(Entity);
            StringBuilder sb = new StringBuilder();

            Assert.IsTrue(KnownAssemblies.Count > 0, "No modules defined to scan.");
            
            foreach (var assembly in KnownAssemblies)
            {                
                var entityTypes = assembly.GetExportedTypes().Where(entityBaseType.IsAssignableFrom).ToList();
                foreach (var entityType in entityTypes)
                {
                    var serializationAttribute = entityType.GetCustomAttributes(typeof(SerializableAttribute), false);
                    if (serializationAttribute.Length == 0)
                    {
                        sb.AppendLine(
                            string.Format(
                                "The {0} entity from the {1} assembly should be decorated with the Serializable attribute.", entityType.Name, assembly.GetName().Name));
                    }
                }
            }

            if (sb.Length > 0)
            {
                Assert.Fail(sb.ToString());
            }
        }
    }
}
