using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Blog.Accessors;
using BetterCms.Module.Blog.Helpers.Extensions;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Helpers.Extensions;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.ViewModels.Cms;
using BetterCms.Module.Root.ViewModels.Option;

using NUnit.Framework;

using BlogRenderingPageProperties = BetterCms.Module.Blog.Mvc.PageHtmlRenderer.RenderingPageProperties;
using PagesRenderingPageProperties = BetterCms.Module.Pages.Mvc.PageHtmlRenderer.RenderingPageProperties;

namespace BetterCms.Test.Module.Root.HelperTests
{
    public class PageHtmlRendererTests : TestBase
    {
        private static class OptionNames
        {
            public const string Text = "OptionText";
            public const string Float = "OptionFloat";
            public const string Date = "OptionDate";
        }

        [Test]
        public void Should_Return_Correct_Page_Title()
        {
            var html = "{{" + RenderingPageProperties.PageTitle + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.Title);
        }
        
        [Test]
        public void Should_Return_Correct_Page_Url()
        {
            var html = "{{" + RenderingPageProperties.PageUrl + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.PageUrl);
        }
        
        [Test]
        public void Should_Return_Correct_Page_Id()
        {
            var html = "{{" + RenderingPageProperties.PageId + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.Id.ToString());
        }
        
        [Test]
        public void Should_Return_Correct_Page_MetaTitle()
        {
            var html = "{{" + RenderingPageProperties.MetaTitle + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.MetaTitle);
        }
        
        [Test]
        public void Should_Return_Correct_Page_MetaKeywords()
        {
            var html = "{{" + RenderingPageProperties.MetaKeywords + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.MetaKeywords);
        }
        
        [Test]
        public void Should_Return_Correct_Page_MetaDescription()
        {
            var html = "{{" + RenderingPageProperties.MetaDescription + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.MetaDescription);
        }
        
        [Test]
        public void Should_Return_Correct_Page_PageCreatedOn_Default()
        {
            var html = "{{" + RenderingPageProperties.PageCreatedOn + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.CreatedOn.ToString(System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern));
        }
        
        [Test]
        public void Should_Return_Correct_Page_PageCreatedOn_Formatted()
        {
            var html = "{{" + RenderingPageProperties.PageCreatedOn + ":yyyy MM}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.CreatedOn.ToString("yyyy MM"));
        }
        
        [Test]
        public void Should_Return_Correct_Page_PageModifiedOn_Default()
        {
            var html = "{{" + RenderingPageProperties.PageModifiedOn + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.ModifiedOn.ToString(System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern));
        }
        
        [Test]
        public void Should_Return_Correct_Page_PageModifiedOn_Formatted()
        {
            var html = "{{" + RenderingPageProperties.PageModifiedOn + ":yyyy MM}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.ModifiedOn.ToString("yyyy MM"));
        }
        
        [Test]
        public void Should_Return_Correct_Page_TextOption()
        {
            var html = "{{" + RenderingPageProperties.PageOption + ":" + OptionNames.Text + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.Options.First(o => o.Key == OptionNames.Text).Value);
        }
        
        [Test]
        public void Should_Return_Correct_Page_DateOption_Default()
        {
            var html = "{{" + RenderingPageProperties.PageOption + ":" + OptionNames.Date + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, ((DateTime)model.Options.First(o => o.Key == OptionNames.Date).Value).ToString(System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern));
        }
        
        [Test]
        public void Should_Return_Correct_Page_DateOption_Formatted()
        {
            var html = "{{" + RenderingPageProperties.PageOption + ":" + OptionNames.Date + ":yyyy MM}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, ((DateTime)model.Options.First(o => o.Key == OptionNames.Date).Value).ToString("yyyy MM"));
        }
        
        [Test]
        public void Should_Return_Correct_Page_FloatOption_Formatted()
        {
            var html = "{{" + RenderingPageProperties.PageOption + ":" + OptionNames.Float + ":.0000}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, ((decimal)model.Options.First(o => o.Key == OptionNames.Float).Value).ToString(".0000"));
        }

        [Test]
        public void Should_Return_Correct_Page_Category()
        {
            var html = "{{" + PagesRenderingPageProperties.PageCategory + "}}";
            var model = CreateTestViewModel(true);
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.GetPageCategoryModel().Name);
        }

        [Test]
        public void Should_Return_Empty_Page_Category()
        {
            var html = "{{" + PagesRenderingPageProperties.PageCategory + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, string.Empty);
        }
        
        [Test]
        public void Should_Return_Correct_Page_MainImageUrl()
        {
            var html = "{{" + PagesRenderingPageProperties.MainImageUrl + "}}";
            var model = CreateTestViewModel(true);
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.GetPageMainImageModel().PublicUrl);
        }
        
        [Test]
        public void Should_Return_Empty_Page_MainImageUrl()
        {
            var html = "{{" + PagesRenderingPageProperties.MainImageUrl + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, string.Empty);
        }
        
        [Test]
        public void Should_Return_Empty_Page_SecondaryImageUrl()
        {
            var html = "{{" + PagesRenderingPageProperties.SecondaryImageUrl + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, string.Empty);
        }

        [Test]
        public void Should_Return_Correct_Page_SecondaryImageUrl()
        {
            var html = "{{" + PagesRenderingPageProperties.SecondaryImageUrl + "}}";
            var model = CreateTestViewModel(true);
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.GetPageSecondaryImageModel().PublicUrl);
        }

        [Test]
        public void Should_Return_Empty_Page_FeaturedImageUrl()
        {
            var html = "{{" + PagesRenderingPageProperties.FeaturedImageUrl + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, string.Empty);
        }

        [Test]
        public void Should_Return_Correct_Page_FeaturedImageUrl()
        {
            var html = "{{" + PagesRenderingPageProperties.FeaturedImageUrl + "}}";
            var model = CreateTestViewModel(true);
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.GetPageFeaturedImageModel().PublicUrl);
        }

        [Test]
        public void Should_Return_Empty_Page_Author()
        {
            var html = "{{" + BlogRenderingPageProperties.BlogAuthor + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, string.Empty);
        }

        [Test]
        public void Should_Return_Correct_Page_Author()
        {
            var html = "{{" + BlogRenderingPageProperties.BlogAuthor + "}}";
            var model = CreateTestViewModel(true);
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.GetBlogPostAuthorModel().Name);
        }

        [Test]
        public void Should_Return_Empty_Page_BlogActivationDate()
        {
            var html = "{{" + BlogRenderingPageProperties.BlogActivationDate + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, string.Empty);
        }
        
        [Test]
        public void Should_Return_Correct_Page_BlogActivationDate_Default()
        {
            var html = "{{" + BlogRenderingPageProperties.BlogActivationDate + "}}";
            var model = CreateTestViewModel(true);
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.GetBlogPostModel().ActivationDate.ToString(System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern));
        }
        
        [Test]
        public void Should_Return_Correct_Page_BlogActivationDate_Formatted()
        {
            var html = "{{" + BlogRenderingPageProperties.BlogActivationDate + ":yyyy MM}}";
            var model = CreateTestViewModel(true);
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.GetBlogPostModel().ActivationDate.ToString("yyyy MM"));
        }
        
        [Test]
        public void Should_Return_Empty_Page_BlogExpirationDate()
        {
            var html = "{{" + BlogRenderingPageProperties.BlogExpirationDate + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, string.Empty);
        }

        [Test]
        public void Should_Return_Correct_Page_BlogExpirationDate_Default()
        {
            var html = "{{" + BlogRenderingPageProperties.BlogExpirationDate + "}}";
            var model = CreateTestViewModel(true);
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.GetBlogPostModel().ExpirationDate.Value.ToString(System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern));
        }
        
        [Test]
        public void Should_Return_Correct_Page_BlogExpirationDate_Formatted()
        {
            var html = "{{" + BlogRenderingPageProperties.BlogExpirationDate + ":yyyy MM}}";
            var model = CreateTestViewModel(true);
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.GetBlogPostModel().ExpirationDate.Value.ToString("yyyy MM"));
        }


        /// <summary>
        /// Gets the replaced HTML.
        /// </summary>
        /// <returns>HTML with replaced view model values</returns>
        private string GetReplacedHtml(string html, RenderPageViewModel model)
        {
            var helper = new PageHtmlRenderer(new StringBuilder(html), model);
            return helper.GetReplacedHtml().ToString();
        }

        /// <summary>
        /// Creates the test view model.
        /// </summary>
        /// <returns>Test view model</returns>
        private RenderPageViewModel CreateTestViewModel(bool extendModel = false)
        {
            var entity = new BlogPost
            {
                Title = "Fake Page Title",
                PageUrl = "/Fake/Page/Url/s",
                Id = new Guid("DB4C3C70-F5F3-44A1-9472-6155A9A77D89"),
                CreatedOn = new DateTime(2010, 11, 15),
                ModifiedOn = new DateTime(2012, 12, 3),
                CreatedByUser = "Fake Page Creator",
                ModifiedByUser = "Fake Page Modifier",
                MetaTitle = "Fake Page Meta Title",
                MetaKeywords = "Fake Page Meta Keywords",
                MetaDescription = "Fake Page MetaDescription",
                ActivationDate = new DateTime(2012, 5, 12),
                ExpirationDate = new DateTime(2013, 4, 18)
            };

            if (extendModel)
            {
                entity.Categories = new List<PageCategory>() { new PageCategory() { Category = new Category { Name = "Fake Category Name" }, Page = entity } };
                entity.Author = new Author { Name = "Fake Author Name" };
                entity.Image = new MediaImage { PublicUrl = "/Fake/Main/Image/Url/" };
                entity.SecondaryImage = new MediaImage { PublicUrl = "/Fake/Secondary/Image/Url/" };
                entity.FeaturedImage = new MediaImage { PublicUrl = "/Fake/Featured/Image/Url/" };
                entity.ActivationDate = new DateTime();

                var content = new BlogPostContent { ActivationDate = new DateTime(2012, 5, 12), ExpirationDate = new DateTime(2013, 4, 18) };
                var pageContent = new PageContent { Content = content, Page = entity };
                entity.PageContents = new List<PageContent> { pageContent };
            }

            var model = new RenderPageViewModel(entity)
                            {
                                Options = new List<IOptionValue>
                                              {
                                                  new OptionValueViewModel
                                                      {
                                                          OptionKey = OptionNames.Text,
                                                          OptionValue = "Fake Option Value",
                                                          Type = OptionType.Text
                                                      },

                                                  new OptionValueViewModel
                                                      {
                                                          OptionKey = OptionNames.Float,
                                                          OptionValue = 10.123456M,
                                                          Type = OptionType.Float
                                                      },

                                                  new OptionValueViewModel
                                                      {
                                                          OptionKey = OptionNames.Date,
                                                          OptionValue = new DateTime(2009, 4, 27),
                                                          Type = OptionType.DateTime
                                                      }
                                              }
                            };

            if (extendModel)
            {
                model.Contents = new List<PageContentProjection>();
                model.Contents.Add(
                    new PageContentProjection(
                        entity.PageContents[0],
                        entity.PageContents[0].Content,
                        new BlogPostContentAccessor((BlogPostContent)entity.PageContents[0].Content, new List<IOptionValue>())));
                model.ExtendWithPageData(entity);
                model.ExtendWithBlogData(entity);
            }

            return model;
        }
    }
}
