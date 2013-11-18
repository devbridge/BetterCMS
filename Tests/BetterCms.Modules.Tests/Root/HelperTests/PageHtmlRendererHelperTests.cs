using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.ViewModels.Cms;
using BetterCms.Module.Root.ViewModels.Option;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.HelperTests
{
    public class PageHtmlRendererHelperTests : TestBase
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
            var html = "{{" + PageHtmlRendererHelper.ReplacementIds.PageTitle + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.Title);
        }
        
        [Test]
        public void Should_Return_Correct_Page_Url()
        {
            var html = "{{" + PageHtmlRendererHelper.ReplacementIds.PageUrl + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.PageUrl);
        }
        
        [Test]
        public void Should_Return_Correct_Page_Id()
        {
            var html = "{{" + PageHtmlRendererHelper.ReplacementIds.PageId + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.Id.ToString());
        }
        
        [Test]
        public void Should_Return_Correct_Page_MetaTitle()
        {
            var html = "{{" + PageHtmlRendererHelper.ReplacementIds.MetaTitle + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.MetaTitle);
        }
        
        [Test]
        public void Should_Return_Correct_Page_MetaKeywords()
        {
            var html = "{{" + PageHtmlRendererHelper.ReplacementIds.MetaKeywords + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.MetaKeywords);
        }
        
        [Test]
        public void Should_Return_Correct_Page_MetaDescription()
        {
            var html = "{{" + PageHtmlRendererHelper.ReplacementIds.MetaDescription + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.MetaDescription);
        }
        
        [Test]
        public void Should_Return_Correct_Page_PageCreatedOn_Default()
        {
            var html = "{{" + PageHtmlRendererHelper.ReplacementIds.PageCreatedOn + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.CreatedOn.ToString(CultureInfo.InvariantCulture));
        }
        
        [Test]
        public void Should_Return_Correct_Page_PageCreatedOn_Formatted()
        {
            var html = "{{" + PageHtmlRendererHelper.ReplacementIds.PageCreatedOn + ":yyyy MM}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.CreatedOn.ToString("yyyy MM"));
        }
        
        [Test]
        public void Should_Return_Correct_Page_PageModifiedOn_Default()
        {
            var html = "{{" + PageHtmlRendererHelper.ReplacementIds.PageModifiedOn + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.ModifiedOn.ToString(CultureInfo.InvariantCulture));
        }
        
        [Test]
        public void Should_Return_Correct_Page_PageModifiedOn_Formatted()
        {
            var html = "{{" + PageHtmlRendererHelper.ReplacementIds.PageModifiedOn + ":yyyy MM}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.ModifiedOn.ToString("yyyy MM"));
        }
        
        [Test]
        public void Should_Return_Correct_Page_TextOption()
        {
            var html = "{{" + PageHtmlRendererHelper.ReplacementIds.PageOption + ":" + OptionNames.Text + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, model.Options.First(o => o.Key == OptionNames.Text).Value);
        }
        
        [Test]
        public void Should_Return_Correct_Page_DateOption_Default()
        {
            var html = "{{" + PageHtmlRendererHelper.ReplacementIds.PageOption + ":" + OptionNames.Date + "}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, ((DateTime)model.Options.First(o => o.Key == OptionNames.Date).Value).ToString(CultureInfo.InvariantCulture));
        }
        
        [Test]
        public void Should_Return_Correct_Page_DateOption_Formatted()
        {
            var html = "{{" + PageHtmlRendererHelper.ReplacementIds.PageOption + ":" + OptionNames.Date + ":yyyy MM}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, ((DateTime)model.Options.First(o => o.Key == OptionNames.Date).Value).ToString("yyyy MM"));
        }
        
        [Test]
        public void Should_Return_Correct_Page_FloatOption_Formatted()
        {
            var html = "{{" + PageHtmlRendererHelper.ReplacementIds.PageOption + ":" + OptionNames.Float + ":.0000}}";
            var model = CreateTestViewModel();
            html = GetReplacedHtml(html, model);

            Assert.AreEqual(html, ((decimal)model.Options.First(o => o.Key == OptionNames.Float).Value).ToString(".0000"));
        }

        /// <summary>
        /// Gets the replaced HTML.
        /// </summary>
        /// <returns>HTML with replaced view model values</returns>
        private string GetReplacedHtml(string html, RenderPageViewModel model)
        {
            var helper = new PageHtmlRendererHelper(new StringBuilder(html), model);
            return helper.GetReplacedHtml().ToString();
        }

        /// <summary>
        /// Creates the test view model.
        /// </summary>
        /// <returns>Test view model</returns>
        private RenderPageViewModel CreateTestViewModel()
        {
            var model = new RenderPageViewModel
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

            return model;
        }
    }
}
