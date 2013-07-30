using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BetterCms.Test.Module.Common
{
    /// <summary>
    /// View model to string test
    /// </summary>
    [TestFixture]
    public class ViewModelToStringTest : TestBase
    {
        /// <summary>
        /// Should_be_overrided_toes the string_in_all_view models.
        /// </summary>
        [Test]
        public void All_View_Models_Should_Override_ToString_Method()
        {
            List<string> viewModelNames = new List<string>();

            foreach (Assembly assembly in KnownAssemblies)
            {
                IList<Type> types = assembly.GetTypes().Where(type => type.IsClass && type.Name.ToLower().EndsWith("viewmodel")).ToList();
                if (types.Count > 0)
                {
                    foreach (Type type in types)
                    {
                        MethodInfo method = type.GetMethod("ToString", BindingFlags.Instance | BindingFlags.Public);
                        if (method.DeclaringType != type)
                        {
                            viewModelNames.Add(type.FullName);
                        }
                    }
                }
            }

            // Format view models name, which didn't have overrided ToString() method:
            StringBuilder builder = new StringBuilder();
            foreach (string name in viewModelNames)
            {
                builder.Append(name + ", ");
            }

            Assert.AreEqual(0, viewModelNames.Count, string.Format("Not all view models has ToString override methods: {0}", builder));
        }
    }
}
