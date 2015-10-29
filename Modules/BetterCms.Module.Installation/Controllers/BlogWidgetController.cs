using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using BetterCms.Core.Extensions;
using BetterCms.Module.Api;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;
using BetterCms.Module.Api.Operations.Blog.BlogPosts;
using BetterCms.Module.Api.Operations.Root.Categories;
using BetterCms.Module.Api.Operations.Root.Categories.Category;
using BetterCms.Module.Installation.Models.Blog;
using BetterCms.Module.Root.Mvc;
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

            var isPagingEnabled = model.Options.Where(x => x.Key == "ShowPager").Select(x => x.CastValueOrDefault<bool>()).FirstOrDefault();
            var pageSize = model.Options.Where(x => x.Key == "PageSize").Select(x => x.CastValueOrDefault<int>()).FirstOrDefault();
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
                ShowAuthor = model.Options.Where(x => x.Key == "ShowAuthor").Select(x => x.CastValueOrDefault<bool>()).FirstOrDefault(),
                ShowDate = model.Options.Where(x => x.Key == "ShowDate").Select(x => x.CastValueOrDefault<bool>()).FirstOrDefault(),
                ShowCategories = model.Options.Where(x => x.Key == "ShowCategories").Select(x => x.CastValueOrDefault<bool>()).FirstOrDefault(),
                ShowTags = model.Options.Where(x => x.Key == "ShowTags").Select(x => x.CastValueOrDefault<bool>()).FirstOrDefault(),
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
                var useSpecificCategoryTree = model.Options.Where(x => x.Key == "UseSpecificCategoryTree").Select(x => x.CastValueOrDefault<bool>()).FirstOrDefault();
                var categoryTreeName = model.Options.Where(x => x.Key == "CategoryTreeName").Select(x => x.CastValueOrDefault<string>()).FirstOrDefault();

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