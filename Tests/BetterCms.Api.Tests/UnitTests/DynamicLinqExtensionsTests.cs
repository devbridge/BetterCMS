// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DynamicLinqExtensionsTests.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations;

using NUnit.Framework;
using BetterCms.Module.Api.Helpers;

namespace BetterCms.Api.Tests.UnitTests
{
    public class DynamicLinqExtensionsTests
    {
        private class GuidConstants
        {
            public const string Empty1 = "Empty 1";
            public const string Empty2 = "Empty 2";
            public const string Nullable1 = "Nullable 1";
            public const string Nullable2 = "Nullable 2";
            public const string Normal1 = "Normal 1";
            public const string Normal2 = "Normal 2";
            public const string Guid1 = "DF334B62-5949-4472-A375-372BB53D9817";
            public const string Guid2 = "1B6FA396-C60C-4A6C-99C6-5BC54EAAB384";
        }

        [Test]
        public void FilterByNullableGuidTest()
        {
            var models = CreateModelsForGuidTests();

            var result = models.Where("NullableGuid == null").ToList();
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 2);
            Assert.IsTrue(result.Any(m => m.Title == GuidConstants.Nullable1));
            Assert.IsTrue(result.Any(m => m.Title == GuidConstants.Nullable2));
        }
        
        [Test]
        public void FilterByNotNullableGuidTest()
        {
            var models = CreateModelsForGuidTests();

            var result = models.Where("NullableGuid != null").ToList();
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 4);
            Assert.IsTrue(result.Any(m => m.Title == GuidConstants.Empty1));
            Assert.IsTrue(result.Any(m => m.Title == GuidConstants.Empty2));
            Assert.IsTrue(result.Any(m => m.Title == GuidConstants.Normal1));
            Assert.IsTrue(result.Any(m => m.Title == GuidConstants.Normal2));
        }
        
        [Test]
        public void FilterByEqualGuid()
        {
            var models = CreateModelsForGuidTests();

            var result = models.Where("NullableGuid == @0", new Guid(GuidConstants.Guid1)).ToList();
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 1);
            Assert.IsTrue(result.Any(m => m.Title == GuidConstants.Normal1));
        }
        
        [Test]
        public void FilterByNotEqualGuid()
        {
            var models = CreateModelsForGuidTests();

            var result = models.Where("NullableGuid != @0", new Guid(GuidConstants.Guid1)).ToList();
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 5);
            Assert.IsFalse(result.Any(m => m.Title == GuidConstants.Normal1));
        }

        private IQueryable<TestModel> CreateModelsForGuidTests()
        {
            return new List<TestModel>
                    {
                        new TestModel { NullableGuid = new Guid(), Title = GuidConstants.Empty1 },
                        new TestModel { NullableGuid = new Guid(), Title = GuidConstants.Empty2 },
                        new TestModel { NullableGuid = new Guid(GuidConstants.Guid1), Title = GuidConstants.Normal1 },
                        new TestModel { NullableGuid = new Guid(GuidConstants.Guid2), Title = GuidConstants.Normal2 },
                        new TestModel { NullableGuid = null, Title = GuidConstants.Nullable1 },
                        new TestModel { NullableGuid = null, Title = GuidConstants.Nullable2 },
                    }.AsQueryable();
        }

        private class TestModel : ModelBase
        {
            public string Title { get; set; }

            public Guid? NullableGuid { get; set; }
        }
    }
}