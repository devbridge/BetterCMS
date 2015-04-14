using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;

using BetterCms.Configuration;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Helpers;

using BlogML.Xml;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Web;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Blog.ServiceTests
{
    public class BlogMLServiceTests : TestBase
    {
        private const string BlogMLImportFile = "BetterCms.Test.Module.Contents.BlogML.BlogMLImport1.xml";

        [Test]
        public void ShouldImportBlogPostsFromFile_NoRedirects()
        {
            Assert.Ignore("TODO: fix when service will be finished");

            var repository = CreateRepository();
            repository.Setup(x => x.Save(It.IsAny<Redirect>())).Callback<Redirect>(x => {
                throw new AssertionException("Redirect shouldn't be created");
            });

            var redirectsCreated = 0;
            repository.Setup(x => x.Save(It.IsAny<Redirect>())).Callback<Redirect>(x =>
            {
                redirectsCreated++;
            });

            var tested = false;
            string[] errorMesages;
            var blogService = CreateBlogService();
            blogService
                .Setup(x => x.SaveBlogPost(It.IsAny<BlogPostViewModel>(), null, It.IsAny<IPrincipal>(), out errorMesages, true))
                .Returns((BlogPostViewModel x, IPrincipal principal) =>
                    {
                        AssertBlogPostUrl(x);
                        if (x.BlogUrl == "cs-dev-guide-send-emails")
                        {
                            tested = true;
                            AssertBlogPost(x);
                        }

                        return new BlogPost
                            {
                                Title = x.Title,
                                PageUrl = x.BlogUrl,
                                Id = Guid.NewGuid()
                            };
                    });

            var importService = GetBlogService(repository.Object, blogService.Object);
            var file = CreateTemporaryFile(BlogMLImportFile);

            var blogsML = importService.DeserializeXMLFile(file);
            var blogs = GetImportingBogPosts(blogsML.Posts);
            var results = importService.ImportBlogs(blogsML, blogs, GetPrincipal());

            Assert.IsTrue(results.All(r => r.Success));
            Assert.IsTrue(tested);
            Assert.AreEqual(redirectsCreated, 0);

            DeleteTemporaryFile(file);
        }
        
        [Test]
        public void ShouldImportBlogPostsFromFile_NoOriginalUrls_CreateRedirects()
        {
            Assert.Ignore("TODO: fix when service will be finished");

            var repository = CreateRepository();
            var redirectsCreated = 0;
            repository.Setup(x => x.Save(It.IsAny<Redirect>())).Callback<Redirect>(x =>
                {
                    redirectsCreated++;
                });

            var tested = false;
            string[] errorMesages;
            var blogService = CreateBlogService();
            blogService
                .Setup(x => x.SaveBlogPost(It.IsAny<BlogPostViewModel>(), null, It.IsAny<IPrincipal>(), out errorMesages, true))
                .Returns((BlogPostViewModel x, IPrincipal principal) =>
                    {
                        AssertBlogPostUrl(x);

                        if (x.BlogUrl == "cs-dev-guide-send-emails")
                        {
                            tested = true;
                            AssertBlogPost(x);
                        }

                        return new BlogPost
                               {
                                   Title = x.Title,
                                   PageUrl = x.BlogUrl,
                                   Id = Guid.NewGuid()
                               };
                    });

            var importService = GetBlogService(repository.Object, blogService.Object);
            var file = CreateTemporaryFile(BlogMLImportFile);

            var blogsML = importService.DeserializeXMLFile(file);
            var posts = GetImportingBogPosts(blogsML.Posts);
            var results = importService.ImportBlogs(blogsML, posts, GetPrincipal(), true);

            Assert.IsTrue(results.All(r => r.Success));
            Assert.AreEqual(redirectsCreated, 4);
            Assert.IsTrue(tested);

            DeleteTemporaryFile(file);
        }

        private List<BlogPostImportResult> GetImportingBogPosts(BlogMLBlog.PostCollection posts)
        {
            var list = new List<BlogPostImportResult>();

            var i = 0;
            foreach (var post in posts)
            {
                list.Add(new BlogPostImportResult
                         {
                             Id = post.ID,
                             Title = post.Title,
                             PageUrl = post.PostUrl + "-test"
                         });

                i++;
                if (i >= 3)
                {
                    break;
                }
            }

            return list;
        }

        private void AssertBlogPostUrl(BlogPostViewModel blog)
        {
            Assert.IsFalse(blog.BlogUrl.Contains(".aspx"));
            Assert.IsFalse(blog.BlogUrl.Contains(".asp"));
            Assert.IsFalse(blog.BlogUrl.Contains(".html"));
            Assert.IsFalse(blog.BlogUrl.Contains(".htm"));
            Assert.IsFalse(blog.BlogUrl.Contains(".php"));
        }
        
        private void AssertBlogPost(BlogPostViewModel blog)
        {
            Assert.AreEqual(blog.LiveFromDate, new DateTime(2006, 09, 05));
            Assert.AreEqual(blog.Title, "CS Dev Guide: Send Emails");
            Assert.AreEqual(blog.IntroText, "CS Dev Guide: Send Emails - Intro Text");
            Assert.AreEqual(blog.DesirableStatus, ContentStatus.Published);
            Assert.AreEqual(blog.Content, "<p>Any web application needs a way to send emails to different kinds of its users.&nbsp; This capability is provided in Community Server from early versions.&nbsp; Sending emails is one of easiest parts of Community Server development.</p><p>To send emails in Community Serve you have two options:&nbsp;using pre-defined email templates for some common situations&nbsp;or create an email manually.</p><p>First option is easy to implement.&nbsp; Community Server has provided several default templates for your emails and you can add your own templates via resource files as well.&nbsp; <em>CommunityServer.Components.Emails</em> namespace has many methods that get some parameters&nbsp;then create and send an email based on the template they have.</p><p>For example you can send a notification email to user to let him know his account is created.</p><div style=\"background: white none repeat scroll 0% 50%; font-size: 10pt; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial; color: black; font-family: courier new\"><p style=\"margin: 0px\"><span style=\"color: blue\">private</span> <span style=\"color: blue\">void</span> SendEmail(<span style=\"color: blue\">string</span> username, <span style=\"color: blue\">string</span> password</p><p style=\"margin: 0px\">&nbsp;&nbsp;&nbsp; , <span style=\"color: blue\">string</span> email)</p><p style=\"margin: 0px\">{</p><p style=\"margin: 0px\">&nbsp;&nbsp;&nbsp; <span style=\"color: teal\">User</span> user = <span style=\"color: blue\">new</span> <span style=\"color: teal\">User</span>();</p><p style=\"margin: 0px\">&nbsp;&nbsp;&nbsp; user.Username = username;</p><p style=\"margin: 0px\">&nbsp;&nbsp;&nbsp; user.Password = password;</p><p style=\"margin: 0px\">&nbsp;&nbsp;&nbsp; user.Email = email;</p><p style=\"margin: 0px\">&nbsp;</p><p style=\"margin: 0px\">&nbsp;&nbsp;&nbsp; <span style=\"color: teal\">Emails</span>.UserCreate(user, password);</p><p style=\"margin: 0px\">}</p></div><p>These methods add emails to queue automatically.&nbsp; You can use <em>Emails.SendQueuedEmails()</em> method to send all queued emails to recipients.&nbsp; This method takes three parameters: an integer value for failure interval, an integer value that specifies the maximum number of tries of sending process&nbsp;failed and a <em>SiteSettings</em> object.</p><div style=\"background: white none repeat scroll 0% 50%; font-size: 10pt; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial; color: black; font-family: courier new\"><p style=\"margin: 0px\"><span style=\"color: blue\">private</span> <span style=\"color: blue\">void</span> SendQueuedEmails()</p><p style=\"margin: 0px\">{</p><p style=\"margin: 0px\">&nbsp;&nbsp;&nbsp; <span style=\"color: teal\">Emails</span>.SendQueuedEmails(5, 5,</p><p style=\"margin: 0px\">&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; <span style=\"color: teal\">CSContext</span>.Current.SiteSettings);</p><p style=\"margin: 0px\">}</p></div><p>Second option to send emails is&nbsp;manual option.&nbsp; This is&nbsp;useful when you want to send an email but it doesn&#39;t fit to Community Server pre-defined templates.</p><p>This option is very similar to sending emails in ASP.NET 1.x.&nbsp; You need to create an instance of System.Web.Mail.MailMessage object, set its appropriate properties and add it to emails queue using Community Server APIs.&nbsp; You know MailMessage object is obsolete in ASP.NET 2.0 but Community Server accepts old objects to be able to work under ASP.NET 1.1.&nbsp; If you need more information about sending emails in ASP.NET 2.0, read my post about my SMTP component, Gopi, from <a href=\"http://nayyeri.net/archive/2006/05/20/Gopi-_2D00_-SMTP-component-for-.NET-2.0.asp\">here</a>.</p><p>Here is a sample of how to create a new MailMessage and add it to emails queue.</p><div style=\"background: white none repeat scroll 0% 50%; font-size: 10pt; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial; color: black; font-family: courier new\"><p style=\"margin: 0px\"><span style=\"color: blue\">private</span> <span style=\"color: blue\">void</span> SendEmailManually()</p><p style=\"margin: 0px\">{</p><p style=\"margin: 0px\">&nbsp;&nbsp;&nbsp; <span style=\"color: teal\">MailMessage</span> mail = <span style=\"color: blue\">new</span> <span style=\"color: teal\">MailMessage</span>();</p><p style=\"margin: 0px\">&nbsp;&nbsp;&nbsp; mail.From = <span style=\"color: maroon\">&quot;sender@server.com&quot;</span>;</p><p style=\"margin: 0px\">&nbsp;&nbsp;&nbsp; mail.To = <span style=\"color: maroon\">&quot;receiver@server.com&quot;</span>;</p><p style=\"margin: 0px\">&nbsp;</p><p style=\"margin: 0px\">&nbsp;&nbsp;&nbsp; mail.Priority = <span style=\"color: teal\">MailPriority</span>.Normal;</p><p style=\"margin: 0px\">&nbsp;&nbsp;&nbsp; mail.BodyFormat = <span style=\"color: teal\">MailFormat</span>.Text;</p><p style=\"margin: 0px\">&nbsp;</p><p style=\"margin: 0px\">&nbsp;&nbsp;&nbsp; mail.Body = <span style=\"color: maroon\">&quot;Long Live Community Server!&quot;</span>;</p><p style=\"margin: 0px\">&nbsp;&nbsp;&nbsp; mail.Subject = <span style=\"color: maroon\">&quot;Test Email&quot;</span>;</p><p style=\"margin: 0px\">&nbsp;</p><p style=\"margin: 0px\">&nbsp;&nbsp;&nbsp; <span style=\"color: teal\">EmailQueueProvider</span>.Instance().QueueEmail(mail);</p><p style=\"margin: 0px\">}</p></div><p>You saw I used <em>EmailQueueProvider.Instance().QueueEmail()</em> method to add my email to queue.&nbsp; You can use <em>EmailQueueProvider.Instance()</em> object to deal with emails queue in Community Server.&nbsp; For instance you can remove an email from emails queue by passing its Guid to <em>EmailQueueProvider.Instance().DeleteQueuedEmail()</em> method.</p><div style=\"background: white none repeat scroll 0% 50%; font-size: 10pt; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial; color: black; font-family: courier new\"><p style=\"margin: 0px\"><span style=\"color: blue\">private</span> <span style=\"color: blue\">void</span> DealWithEmails()</p><p style=\"margin: 0px\">{</p><p style=\"margin: 0px\">&nbsp;&nbsp;&nbsp; <span style=\"color: teal\">EmailQueueProvider</span>.Instance()</p><p style=\"margin: 0px\">&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; .DeleteQueuedEmail(<span style=\"color: teal\">Guid</span>.NewGuid());&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; </p><p style=\"margin: 0px\">}</p></div><p><strong>Now playing: </strong>Modern Talking - You are not alone</p>");
            Assert.AreEqual(blog.AuthorId, new Guid("7D601C36-7130-4031-95BC-578C34328143"));
           // Assert.AreEqual(blog.CategoryId, new Guid("B81ACA8C-93CD-48C0-8B7F-F9ADE3D1BAC8"));
        }

        private IPrincipal GetPrincipal()
        {
            return new GenericPrincipal(new GenericIdentity("TEST"), new string[0]);
        }

        private Mock<IBlogService> CreateBlogService()
        {
            var blogService = new Mock<IBlogService>();
            blogService
                .Setup(x => x.CreateBlogPermalink(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<IEnumerable<Guid>>()))
                .Returns<string>(x => x.Transliterate());

            return blogService;
        }

        private Mock<IRepository> CreateRepository()
        {
            var authors = new List<Author>
                          {
                              new Author {Name = "existing", Id = new Guid("7D601C36-7130-4031-95BC-578C34328143")}
                          };
            var categories = new List<Category>
                          {
                              new Category {Name = "Category 1", Id = new Guid("B81ACA8C-93CD-48C0-8B7F-F9ADE3D1BAC8")},
                              new Category {Name = "Category 2", Id = new Guid("8CC7DCFD-C116-4BA5-B707-3D721097E2EB")}
                          };

            var repository = new Mock<IRepository>();
            repository.Setup(x => x.AsQueryable<Author>()).Returns(authors.AsQueryable());
            repository.Setup(x => x.AsQueryable<Category>()).Returns(categories.AsQueryable());
            repository
                .Setup(x => x.Save(It.IsAny<Author>()))
                .Callback<Author>(x => Assert.AreEqual(x.Name, "admin"));
            repository
                .Setup(x => x.Save(It.IsAny<Category>()))
                .Callback<Category>(x => Assert.AreEqual(x.Name, "Category 3"));

            return repository;
        }

        private DefaultBlogMLService GetBlogService(IRepository repository, IBlogService blogService, IRedirectService redirectService = null)
        {
            if (redirectService == null)
            {
                redirectService = new Mock<IRedirectService>().Object;
            }
            var unitOfWork = new Mock<IUnitOfWork>().Object;
            var pageService = new Mock<IPageService>().Object;
            var cmsConfiguration = new Mock<ICmsConfiguration>().Object;
            var httpContextAccessor = new Mock<IHttpContextAccessor>().Object;
            var urlService = new DefaultUrlService(unitOfWork, new CmsConfigurationSection());

            var importService = new DefaultBlogMLService(repository, urlService, blogService, unitOfWork, redirectService, pageService, cmsConfiguration, httpContextAccessor);

            return importService;
        }

        private string CreateTemporaryFile(string resouceName)
        {
            var file = System.IO.Path.GetTempFileName();
            using (var fileStream = System.IO.File.Create(file))
            {
                using (var input = Assembly.GetExecutingAssembly().GetManifestResourceStream(resouceName))
                {
                    input.CopyTo(fileStream);
                }
            }

            return file;
        }

        private void DeleteTemporaryFile(string file)
        {
            try
            {
                System.IO.File.Delete(file);
            }
            catch
            {
                // Do nothing
                Console.WriteLine("Failed to delete temporary XML file: {0}", file);
            }
        }
    }
}
