using System;
using System.Collections.Generic;

using BetterCms.Module.Api.Operations.Pages.Pages.Page;

using NUnit.Framework;

using System.Linq;
using System.Reflection;

namespace BetterCms.Api.Tests.Common
{
    [TestFixture]
    public class ModelsContractAttributesTest
    {
        [Test]
        public void All_Models_Should_Have_DataContract_Attributes()
        {
            var assembly = Assembly.GetAssembly(typeof(GetPageRequest));
            var allTypes = assembly.GetTypes();

            var models = allTypes.Where(type => type.IsClass && type.Name.EndsWith("Model")).ToList();
            var requests = allTypes.Where(type => type.IsClass && type.Name.EndsWith("Request")).ToList();
            var responses = allTypes.Where(type => type.IsClass && type.Name.EndsWith("Response")).ToList();

            var anyErrors = HasAnyErrors(models);
            anyErrors = anyErrors || HasAnyErrors(requests);
            anyErrors = anyErrors || HasAnyErrors(responses);

            Assert.IsFalse(anyErrors, "Some classes or properties has no data contract attributes.");
        }

        private bool HasAnyErrors(IList<Type> types)
        {
            bool anyErrors = false;

            foreach (var type in types)
            {
                var classAttributes = type.GetCustomAttributes(false);
                if (classAttributes.All(attr => attr.GetType().Name != "DataContractAttribute"))
                {
                    Console.WriteLine("Classs {0} has no DataContractAttribute", type.Name);
                    anyErrors = true;
                }
                
                if (classAttributes.All(attr => attr.GetType().Name != "SerializableAttribute"))
                {
                    Console.WriteLine("Classs {0} has no SerializableAttribute", type.Name);
                    anyErrors = true;
                }

                var properties = type.GetProperties();
                foreach (var property in properties)
                {
                    var propertyAttributes = property.GetCustomAttributes(false);
                    if (!propertyAttributes.Any(attr => attr.GetType().Name == "DataMemberAttribute") && !propertyAttributes.Any(attr => attr.GetType().Name == "DataMemberIgnoreAttribute"))
                    {
                        Console.WriteLine("Property {0}.{1} has no DataMemberAttribute", type.Name, property.Name);
                        anyErrors = true;
                    }
                }
            }

            return anyErrors;
        }
    }
}
