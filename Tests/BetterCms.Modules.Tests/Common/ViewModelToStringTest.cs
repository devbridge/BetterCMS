// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModelToStringTest.cs" company="Devbridge Group LLC">
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
            var viewModelNames = new List<string>();

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
            var builder = new StringBuilder();
            foreach (string name in viewModelNames)
            {
                builder.Append(name + ", ");
            }

            Assert.AreEqual(0, viewModelNames.Count, string.Format("Not all view models has ToString override methods: {0}", builder));
        }
    }
}
