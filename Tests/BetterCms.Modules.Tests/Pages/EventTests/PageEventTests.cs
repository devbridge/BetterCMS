// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageEventTests.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Pages.Models;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.EventTests
{
    [TestFixture]
    public class PageEventTests : IntegrationTestBase
    {
        private bool firedCreated;
        private bool firedDeleted;

        [Test]
        public void Should_Fire_Page_Created_Event()
        {
            firedCreated = false;
            
            Events.PageEvents.Instance.OnPageCreated(new PageProperties());
            System.Threading.Thread.Sleep(10);
            Assert.IsFalse(firedCreated);

            Events.PageEvents.Instance.PageCreated += delegate { firedCreated = true; };
            
            Events.PageEvents.Instance.OnPageCreated(new PageProperties());
            System.Threading.Thread.Sleep(10);
            Assert.IsTrue(firedCreated);
        }
        
        [Test]
        public void Should_Fire_Page_Deleted_Event()
        {
            firedDeleted = false;

            Events.PageEvents.Instance.OnPageDeleted(new PageProperties());
            System.Threading.Thread.Sleep(10);
            Assert.IsFalse(firedDeleted);

            Events.PageEvents.Instance.PageDeleted += delegate { firedDeleted = true; };

            Events.PageEvents.Instance.OnPageDeleted(new PageProperties());
            System.Threading.Thread.Sleep(10);
            Assert.IsTrue(firedDeleted);
        }
    }
}
