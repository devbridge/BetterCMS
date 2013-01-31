using System;
using System.Linq;
using System.Reflection;

using BetterCms.Core.Models;
using BetterCms.Module.Pages;
using BetterCms.Module.Root;

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

            Assert.IsTrue(KnownAssemblies.Count > 0, "No modules defined to scan.");

            foreach (var assembly in KnownAssemblies)
            {                
                var entityTypes = assembly.GetExportedTypes().Where(entityBaseType.IsAssignableFrom).ToList();
                foreach (var entityType in entityTypes)
                {
                    var serializationAttribute = entityType.GetCustomAttributes(typeof(SerializableAttribute), false);
                    Assert.IsTrue(serializationAttribute.Length == 1,
                        string.Format("The {0} entity from the {1} assembly should be decorated with the Serializable attribute.", entityType.Name, assembly.GetName().Name));
                }
            }
        }
    }
}
