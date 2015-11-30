// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlogWidgetController.cs" company="Devbridge Group LLC">
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
using System.Web.Mvc;

using BetterCms.Module.Api;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;
using BetterCms.Module.Api.Operations.Blog.BlogPosts;
using BetterCms.Module.Api.Operations.Root.Categories;
using BetterCms.Module.Api.Operations.Root.Categories.Category;
using BetterCms.Module.Installation.Models.Blog;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.ViewModels.Cms;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Installation.Controllers
{
    [ActionLinkArea(InstallationModuleDescriptor.ModuleAreaName)]
    public class BlogWidgetController : CmsControllerBase
    {
        public ActionResult BlogPosts(RenderWidgetViewModel model)
        {
            IList<BlogItem> posts;

            var isPagingEnabled = model.GetOptionValue<bool>("ShowPager");
            var pageSize = model.GetOptionValue<int>("PageSize");
            var page = Request.QueryString["blogpage"].ToIntOrDefault();
            int postsCount;

            using (var api = ApiFactory.Create())
            {
                var request = new GetBlogPostsModel { Take = pageSize, IncludeTags = true, IncludeCategories = true };

                SortAndFilterRequest(request);

                if (isPagingEnabled)
                {
                    var skipCount = page == 0 ? 0 : (page - 1) * pageSize;
                    request.Skip = skipCount;
                }

                request.Take = pageSize;

                var pages = api.Blog.BlogPosts.Get(new GetBlogPostsRequest { Data = request });

                posts = pages.Data.Items.Select(
                        x => new BlogItem
                        {
                            IntroText = x.IntroText,
                            PublishedOn = x.ActivationDate,
                            Title = x.Title,
                            Url = x.BlogPostUrl,
                            Author = x.AuthorName,
                            Tags = x.Tags,
                            Categories = x.Categories
                        }).ToList();
                postsCount = pages.Data.TotalCount;
            }

            var items = new BlogItemsModel
            {
                Items = posts,
                ShowAuthor = model.GetOptionValue<bool>("ShowAuthor"),
                ShowDate = model.GetOptionValue<bool>("ShowDate"),
                ShowCategories = model.GetOptionValue<bool>("ShowCategories"),
                ShowTags = model.GetOptionValue<bool>("ShowTags"),
                ShowPager = isPagingEnabled,
                NumberOfPages = (int)Math.Ceiling((double)postsCount / pageSize),
                CurrentPage = page > 0 ? page : 1
            };

            return View(items);
        }

        public ActionResult GetCategories(RenderWidgetViewModel model)
        {
            var categories = new List<CategoryItem>();
            using (var api = ApiFactory.Create())
            {
                var useSpecificCategoryTree = model.GetOptionValue<bool>("UseSpecificCategoryTree");
                var categoryTreeName = model.GetOptionValue<string>("CategoryTreeName");

                var treeRequest = new GetCategoryTreesModel();
                var treePages = api.Root.Categories.Get(new GetCategoryTreesRequest { Data = treeRequest });

                var query = treePages.Data.Items.Where(item => item.AvailableFor.Contains(new Guid("75E6C021-1D1F-459E-A416-D18477BF2020")));

                if (useSpecificCategoryTree)
                {
                    query = query.Where(item => item.Name == categoryTreeName);
                }

                var categoryTreeIds = query
                    .Select(item => item.Id)
                    .ToList();

                foreach (var treeId in categoryTreeIds)
                {
                    var request = new GetCategoryTreeRequest { CategoryTreeId = treeId, Data = { IncludeNodes = true } };
                    var pages = api.Root.Category.Get(request);

                    categories.AddRange(pages.Nodes
                        .Where(node => node.ParentId == null)
                        .OrderBy(node => node.Name)
                        .Select(node => new CategoryItem
                    {
                        Name = node.Name,
                        Id = node.Id
                    }));
                }
            }

            return View(categories);
        }

        private void SortAndFilterRequest(GetBlogPostsModel request)
        {
            var tagName = Request.QueryString["blogtag"];
            var categoryName = Request.QueryString["blogcategory"];

            var orFilter = new DataFilter(FilterConnector.Or);
            orFilter.Add("ExpirationDate", null);
            orFilter.Add("ExpirationDate", DateTime.Today, FilterOperation.GreaterOrEqual);

            request.Order.By.Add(new OrderItem("ActivationDate", OrderDirection.Desc));
            request.Order.By.Add(new OrderItem("Id"));
            request.Filter.Add("ActivationDate", DateTime.Today, FilterOperation.LessOrEqual);
            request.Filter.Inner.Add(orFilter);

            if (!string.IsNullOrEmpty(categoryName))
            {
                request.FilterByCategoriesNames.Add(categoryName);
            }

            if (!string.IsNullOrEmpty(tagName))
            {
                request.FilterByTags.Add(tagName);
            }
        }
    }
}