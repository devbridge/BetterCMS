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
using BetterCms.Module.Installation.Models.Blog;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Cms;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Installation.Controllers
{
    [ActionLinkArea(InstallationModuleDescriptor.ModuleAreaName)]
    public class BlogWidgetController: CmsControllerBase
    {
        public ActionResult BlogPosts(RenderWidgetViewModel model)
        {
            IList<BlogItem> posts;

            using (var api = ApiFactory.Create())
            {
                var pageSize = model.Options.Where(x => x.Key == "PageSize").Select(x => x.CastValueOrDefault<int>()).FirstOrDefault();
                var request = new GetBlogPostsModel { Take = pageSize, IncludeTags = true };
                var pages = api.Blog.BlogPosts.Get(new GetBlogPostsRequest { Data = request });

                posts = pages.Data.Items.Select(
                        x => new BlogItem
                        {
                            IntroText = x.IntroText,
                            PublishedOn = x.ActivationDate,
                            Title = x.Title,
                            Url = x.BlogPostUrl,
                            Author = x.AuthorName,
                            Tags = x.Tags
                        }).ToList();
            }

            BlogItemsModel items = new BlogItemsModel
            {
                Items = posts,
                ShowAuthor = model.Options.Where(x => x.Key == "ShowAuthor").Select(x => x.CastValueOrDefault<bool>()).FirstOrDefault(),
                ShowDate = model.Options.Where(x => x.Key == "ShowDate").Select(x => x.CastValueOrDefault<bool>()).FirstOrDefault(),
                ShowCategories = model.Options.Where(x => x.Key == "ShowCategories").Select(x => x.CastValueOrDefault<bool>()).FirstOrDefault(),
                ShowTags = model.Options.Where(x => x.Key == "ShowTags").Select(x => x.CastValueOrDefault<bool>()).FirstOrDefault(),
                ShowPager = model.Options.Where(x => x.Key == "ShowPager").Select(x => x.CastValueOrDefault<bool>()).FirstOrDefault()
            };

            return View(items);
        }

        public ActionResult GetCategories(RenderWidgetViewModel model)
        {
            using (var api = ApiFactory.Create())
            {
                var request = new GetCategoryTreesModel();
                

                api.Root.Categories.Get(new GetCategoryTreesRequest { Data = request });
            }

            return View();
        }

        private void SortAndFilterRequest(GetBlogPostsModel request)
        {
            var tagName = Request.QueryString["tagName"];
            var categoryName = Request.QueryString["categoryName"];

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