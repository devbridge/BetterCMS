using BetterCms.Module.Api;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api
{
    [TestFixture]
    public class ApiFacadeTests : TestBase
    {
        [Test]
        public void ShouldCreateAndDisposeApiFacade()
        {            
            IApiFacade apiContainer;

            using (var api = ApiFactory.Create())
            {
                apiContainer = api;

                Assert.IsNotNull(api.Root);
                Assert.IsNotNull(api.Root.Layout);
                Assert.IsNotNull(api.Root.Layout.Regions);
                Assert.IsNotNull(api.Root.Layouts);
                Assert.IsNotNull(api.Root.Tags);
                Assert.IsNotNull(api.Root.Tag);
                Assert.IsNotNull(api.Root.Categories);
                Assert.IsNotNull(api.Root.Category);
                Assert.IsNotNull(api.Root.Category.Node);
                Assert.IsNotNull(api.Root.Category.Nodes);
                Assert.IsNotNull(api.Root.Category.Tree);
                Assert.IsNotNull(api.Root.Languages);
                Assert.IsNotNull(api.Root.Language);
                Assert.IsNotNull(api.Root.Version);

                Assert.IsNotNull(api.Pages);
                Assert.IsNotNull(api.Pages.Pages);
                Assert.IsNotNull(api.Pages.Page);
                Assert.IsNotNull(api.Pages.Content);
                Assert.IsNotNull(api.Pages.Content.Html);
                Assert.IsNotNull(api.Pages.Content.History);
                Assert.IsNotNull(api.Pages.Content.Draft);
                Assert.IsNotNull(api.Pages.Page.Contents);
                Assert.IsNotNull(api.Pages.Page.Properties);
                Assert.IsNotNull(api.Pages.Redirect);
                Assert.IsNotNull(api.Pages.Redirects);
                Assert.IsNotNull(api.Pages.Widget);
                Assert.IsNotNull(api.Pages.Widget.HtmlContent);
                Assert.IsNotNull(api.Pages.Widget.HtmlContent.Options);
                Assert.IsNotNull(api.Pages.Widget.ServerControl);
                Assert.IsNotNull(api.Pages.Widget.ServerControl.Options);
                Assert.IsNotNull(api.Pages.Widgets);
                Assert.IsNotNull(api.Pages.Sitemap);
                Assert.IsNotNull(api.Pages.Sitemap.Node);
                Assert.IsNotNull(api.Pages.Sitemap.Nodes);
                Assert.IsNotNull(api.Pages.Sitemap.Tree);
                Assert.IsNotNull(api.Pages.SitemapNew);
                Assert.IsNotNull(api.Pages.SitemapNew.Node);
                Assert.IsNotNull(api.Pages.SitemapNew.Nodes);
                Assert.IsNotNull(api.Pages.SitemapNew.Tree);
                Assert.IsNotNull(api.Pages.Sitemaps);

                Assert.IsNotNull(api.Blog);
                Assert.IsNotNull(api.Blog.BlogPost);
                Assert.IsNotNull(api.Blog.BlogPost.Content);
                Assert.IsNotNull(api.Blog.BlogPost.Properties);
                Assert.IsNotNull(api.Blog.BlogPosts);
                Assert.IsNotNull(api.Blog.Author);
                Assert.IsNotNull(api.Blog.Authors);

                Assert.IsNotNull(api.Media);
                Assert.IsNotNull(api.Media.MediaTree);
                Assert.IsNotNull(api.Media.Folders);
                Assert.IsNotNull(api.Media.Image);
                Assert.IsNotNull(api.Media.Images);
                Assert.IsNotNull(api.Media.File);
                Assert.IsNotNull(api.Media.Files);

                Assert.IsNotNull(api.Users);
                Assert.IsNotNull(api.Users.User);
                Assert.IsNotNull(api.Users.Users);
                Assert.IsNotNull(api.Users.Role);
                Assert.IsNotNull(api.Users.Roles);
            }

            Assert.IsNull(apiContainer.Scope);
        }
    }
}