// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationTest.cs" company="Devbridge Group LLC">
//
// Copyright (C) 2015,2016 Devbridge Group LLC
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
//
// Website: https://www.bettercms.com
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
            var sb = new StringBuilder();

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
