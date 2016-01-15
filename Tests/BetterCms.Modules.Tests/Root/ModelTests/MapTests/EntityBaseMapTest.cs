// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityBaseMapTest.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.DataContracts;
using BetterCms.Module.Root.Models;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class EntityBaseFieldsTest : IntegrationTestBase
    {
        /// <summary>
        /// Checks guid.comb
        /// </summary>
        [Test]
        public void Should_Generate_Entity_Id_After_Save()
        {
            // Can be any another entity type.
            var entity = new Tag();
            entity.Name = "test name";

            SaveEntityAndRunAssertionsInTransaction(
                entity,
                resultTag => Assert.AreNotEqual(Guid.Empty, resultTag.Id),
                tagBeforeSave => Assert.AreEqual(Guid.Empty, tagBeforeSave.Id),
                tagAfterSave => Assert.AreNotEqual(Guid.Empty, tagAfterSave.Id));
        }

        [Test]
        public void Should_Apply_CreatedOn_Field_After_Save()
        {
            // Can be any another entity type.
            var entity = new Category();
            entity.Name = "test name";
            entity.CategoryTree = new CategoryTree();
            entity.CategoryTree.Title = "category tree";
            entity.CategoryTree.Categories = new List<Category>();
            SaveEntityAndRunAssertionsInTransaction(
                entity,
                resultEntity =>
                    {
                        Assert.IsNotNullOrEmpty(resultEntity.CreatedByUser);
                        Assert.AreNotEqual(default(DateTime), resultEntity.CreatedOn);
                    },
                entityBeforeSave =>
                    {
                        Assert.IsNullOrEmpty(entityBeforeSave.CreatedByUser);
                        Assert.AreEqual(default(DateTime), entityBeforeSave.CreatedOn);
                    },
                entityAfterSave =>
                    {
                        Assert.IsNotNullOrEmpty(entityAfterSave.CreatedByUser);
                        Assert.AreNotEqual(default(DateTime), entityAfterSave.CreatedOn);
                    });
        }

        [Test]
        public void Should_Apply_ModifiedOn_Field_After_Save()
        {
            // Can be any another entity type.
            var entity = new Category();
            entity.Name = "test name";
            entity.CategoryTree = new CategoryTree();
            entity.CategoryTree.Title = "category tree";
            entity.CategoryTree.Categories = new List<Category>();
            SaveEntityAndRunAssertionsInTransaction(
                entity,
                resultEntity =>
                    {
                        Assert.IsNotNullOrEmpty(resultEntity.ModifiedByUser);
                        Assert.AreNotEqual(default(DateTime), resultEntity.ModifiedOn);
                    },
                entityBeforeSave =>
                    {
                        Assert.IsNullOrEmpty(entityBeforeSave.ModifiedByUser);
                        Assert.AreEqual(default(DateTime), entityBeforeSave.ModifiedOn);
                    },
                entityAfterSave =>
                    {
                        Assert.IsNotNullOrEmpty(entityAfterSave.ModifiedByUser);
                        Assert.AreNotEqual(default(DateTime), entityAfterSave.ModifiedOn);
                    });
        }

        [Test]
        public void Should_Apply_DeletedOn_Field_After_Save()
        {
            // Can be any another entity type.
            var entity = new Category();
            entity.Name = "test name";
            entity.CategoryTree = new CategoryTree();
            entity.CategoryTree.Title = "category tree";
            entity.CategoryTree.Categories = new List<Category>();
            DeleteCreatedEntityAndRunAssertionsInTransaction(
                entity,
                resultEntity =>
                    {
                        Assert.IsTrue(resultEntity.IsDeleted);
                        Assert.IsNotNullOrEmpty(resultEntity.DeletedByUser);
                        Assert.AreNotEqual(default(DateTime), resultEntity.DeletedOn);
                    },
                entityBeforeSave =>
                    {
                        Assert.IsFalse(entity.IsDeleted);
                        Assert.IsNullOrEmpty(entityBeforeSave.DeletedByUser);
                        Assert.AreEqual(default(DateTime?), entityBeforeSave.DeletedOn);
                    },
                entityAfterSave =>
                    {
                        Assert.IsTrue(entityAfterSave.IsDeleted);
                        Assert.IsNotNullOrEmpty(entityAfterSave.DeletedByUser);
                        Assert.AreNotEqual(default(DateTime), entityAfterSave.DeletedOn);
                    });
        }
    }
}
