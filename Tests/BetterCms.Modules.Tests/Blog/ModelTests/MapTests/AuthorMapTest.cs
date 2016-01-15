// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorMapTest.cs" company="Devbridge Group LLC">
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

using Autofac;

using BetterCMS.Module.LuceneSearch.Models;
using BetterCMS.Module.LuceneSearch.Services.ScrapeService;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Module.Api.Operations.Blog.Authors;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Root.Models;

using NUnit.Framework;

namespace BetterCms.Test.Module.Blog.ModelTests.MapTests
{
    [TestFixture]
    public class AuthorMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Author_Successfully()
        {
            var content = TestDataProvider.CreateNewAuthor();
            RunEntityMapTestsInTransaction(content);  
        }
        
        [Test]
        public void Should_Retrieve_AllField_If_Image_Is_Null()
        {
            RunActionInTransaction(
                session =>
                    {
                        var testAuthor = TestDataProvider.CreateNewAuthor();
                        testAuthor.Image = TestDataProvider.CreateNewMediaImage();
                        testAuthor.Image.IsDeleted = true;                        
                        session.SaveOrUpdate(testAuthor);
                        session.Flush();
                        session.Clear();

                        var repository = new DefaultRepository(new DefaultUnitOfWork(session));

                        var model = repository.AsQueryable<Author>()
                            .Where(f => !f.IsDeleted)
                            .Where(f => f.Id == testAuthor.Id)
                            .Select(author => new AuthorModel
                                          {
                                              Id = author.Id,
                                              Version = author.Version,
                                              CreatedBy = author.CreatedByUser,
                                              CreatedOn = author.CreatedOn,
                                              LastModifiedBy = author.ModifiedByUser,
                                              LastModifiedOn = author.ModifiedOn,

                                              Name = author.Name,

                                              ImageId = author.Image != null && !author.Image.IsDeleted ? author.Image.Id : (Guid?)null,
                                              ImageUrl = author.Image != null && !author.Image.IsDeleted ? author.Image.PublicUrl : (string)null,
                                              ImageThumbnailUrl = author.Image != null && !author.Image.IsDeleted ? author.Image.PublicThumbnailUrl : (string)null,
                                              ImageCaption = author.Image != null && !author.Image.IsDeleted ? author.Image.Caption : (string)null

                                          }).FirstOne();

                        Assert.IsNotNull(model);
                        Assert.AreEqual(testAuthor.Id, model.Id);
                        Assert.IsNull(model.ImageId);
                    });
        }
    }
}
