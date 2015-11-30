// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConcurrencyTests.cs" company="Devbridge Group LLC">
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
using Autofac;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Exceptions.DataTier;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ModelTests.ConcurrencyTests
{
    [TestFixture]
    public class ConcurrencyTests : TestBase
    {
        [Test]
        public void TestConcurencySaveWithoutException()
        {
            var sessionFactory = this.Container.Resolve<ISessionFactoryProvider>();
            using (var session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    // Create layout
                    var layout = this.TestDataProvider.CreateNewLayout();
                    session.Save(layout);
                    session.Flush();

                    // Edit layout
                    layout.Name = "Changed 1";
                    session.Save(layout);
                    session.Flush();

                    // Edit layout
                    layout.Name = "Changed 2";
                    session.Save(layout);
                    session.Flush();
                }
            }
        }

        [Test]
        [ExpectedException(typeof(ConcurrentDataException))]
        public void TestConcurencySaveWithException()
        {
            var sessionFactory = this.Container.Resolve<ISessionFactoryProvider>();
            using (var session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    // Create layout
                    var layout = this.TestDataProvider.CreateNewLayout();
                    session.Save(layout);
                    session.Flush();

                    // Edit layout
                    layout.Name = "Changed 1";
                    session.Save(layout);
                    session.Flush();

                    // Edit layout
                    layout.Name = "Changed 2";
                    layout.Version = 99999;
                    session.Save(layout);
                    session.Flush();
                }
            }
        }
    }
}
