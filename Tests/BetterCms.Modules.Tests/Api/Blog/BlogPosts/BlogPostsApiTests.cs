using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using BetterCms.Core.Models;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Blog.BlogPosts
{
    public class BlogPostsApiTests : ApiCrudIntegrationTestBase<
        SaveBlogPostPropertiesModel, BlogPostPropertiesModel,
        PostBlogPostPropertiesRequest, PostBlogPostPropertiesResponse,
        GetBlogPostPropertiesRequest, GetBlogPostPropertiesResponse,
        PutBlogPostPropertiesRequest, PutBlogPostPropertiesResponse,
        DeleteBlogPostPropertiesRequest, DeleteBlogPostPropertiesResponse>
    {
        private Layout layout;
        private Layout defaultLayout;
        private Page masterPage;
        private Region region;
        private IPrincipal principal;

        [SetUp]
        public void SetUp()
        {
            principal = System.Threading.Thread.CurrentPrincipal;
            SetCurrentPrincipal(RootModuleConstants.UserRoles.AllRoles);
        }

        [TearDown]
        public void TearDown()
        {
            System.Threading.Thread.CurrentPrincipal = principal;
        }

        [Test]
        public void Should_CRUD_BlogPost_WithLayout_Successfully()
        {
            // Attach to events
            Events.BlogEvents.Instance.BlogCreated += Instance_EntityCreated;
            Events.BlogEvents.Instance.BlogUpdated += Instance_EntityUpdated;
            Events.BlogEvents.Instance.BlogDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction(
                (api, session) =>
                {
                    masterPage = null;
                    layout = TestDataProvider.CreateNewLayout();
                    region = TestDataProvider.CreateNewRegion();
                    
                    var layoutRegion = new LayoutRegion { Layout = layout, Region = region };
                    layout.LayoutRegions = new[] { layoutRegion };

                    session.SaveOrUpdate(region);
                    session.SaveOrUpdate(layout);
                    session.SaveOrUpdate(layoutRegion);

                    Run(session, api.Blog.BlogPost.Properties.Post, api.Blog.BlogPost.Properties.Get, api.Blog.BlogPost.Properties.Put, api.Blog.BlogPost.Properties.Delete);
                });

            // Detach from events
            Events.BlogEvents.Instance.BlogCreated -= Instance_EntityCreated;
            Events.BlogEvents.Instance.BlogUpdated -= Instance_EntityUpdated;
            Events.BlogEvents.Instance.BlogDeleted -= Instance_EntityDeleted;
        }
        
        [Test]
        public void Should_CRUD_BlogPost_WithLayout_Successfully_WithIdSpecified()
        {
            // Attach to events
            Events.BlogEvents.Instance.BlogCreated += Instance_EntityCreated;
            Events.BlogEvents.Instance.BlogUpdated += Instance_EntityUpdated;
            Events.BlogEvents.Instance.BlogDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction(
                (api, session) =>
                {
                    masterPage = null;
                    layout = TestDataProvider.CreateNewLayout();
                    region = TestDataProvider.CreateNewRegion();
                    
                    var layoutRegion = new LayoutRegion { Layout = layout, Region = region };
                    layout.LayoutRegions = new[] { layoutRegion };

                    session.SaveOrUpdate(region);
                    session.SaveOrUpdate(layout);
                    session.SaveOrUpdate(layoutRegion);

                    RunWithIdSpecified(session, api.Blog.BlogPost.Properties.Get, api.Blog.BlogPost.Properties.Put, api.Blog.BlogPost.Properties.Delete);
                });

            // Detach from events
            Events.BlogEvents.Instance.BlogCreated -= Instance_EntityCreated;
            Events.BlogEvents.Instance.BlogUpdated -= Instance_EntityUpdated;
            Events.BlogEvents.Instance.BlogDeleted -= Instance_EntityDeleted;
        }
        
        [Test]
        public void Should_CRUD_BlogPost_WithMasterPage_Successfully()
        {
            // Attach to events
            Events.BlogEvents.Instance.BlogCreated += Instance_EntityCreated;
            Events.BlogEvents.Instance.BlogUpdated += Instance_EntityUpdated;
            Events.BlogEvents.Instance.BlogDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction(
                (api, session) =>
                {
                    masterPage = TestDataProvider.CreateNewPage();
                    masterPage.IsMasterPage = true;
                    layout = null;

                    var htmlContent = TestDataProvider.CreateNewHtmlContent(100);
                    region = TestDataProvider.CreateNewRegion();
                    var contentRegion = new ContentRegion { Content = htmlContent, Region = region };
                    htmlContent.ContentRegions = new[] { contentRegion };

                    var pageContent = new PageContent { Page = masterPage, Content = htmlContent, Region = region };

                    session.SaveOrUpdate(region);
                    session.SaveOrUpdate(htmlContent);
                    session.SaveOrUpdate(masterPage);
                    session.SaveOrUpdate(pageContent);

                    Run(session, api.Blog.BlogPost.Properties.Post, api.Blog.BlogPost.Properties.Get, api.Blog.BlogPost.Properties.Put, api.Blog.BlogPost.Properties.Delete);
                });

            // Detach from events
            Events.BlogEvents.Instance.BlogCreated -= Instance_EntityCreated;
            Events.BlogEvents.Instance.BlogUpdated -= Instance_EntityUpdated;
            Events.BlogEvents.Instance.BlogDeleted -= Instance_EntityDeleted;
        }
        
        [Test]
        public void Should_CRUD_BlogPost_WithNoLayoutSpecified_Successfully()
        {
            // Attach to events
            Events.BlogEvents.Instance.BlogCreated += Instance_EntityCreated;
            Events.BlogEvents.Instance.BlogUpdated += Instance_EntityUpdated;
            Events.BlogEvents.Instance.BlogDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction(
                (api, session) =>
                {
                    masterPage = null;
                    layout = null;

                    masterPage = null;
                    defaultLayout = TestDataProvider.CreateNewLayout();
                    region = TestDataProvider.CreateNewRegion();

                    var layoutRegion = new LayoutRegion { Layout = defaultLayout, Region = region };
                    defaultLayout.LayoutRegions = new[] { layoutRegion };

                    session.SaveOrUpdate(region);
                    session.SaveOrUpdate(defaultLayout);
                    session.SaveOrUpdate(layoutRegion);

                    // Set default layout
                    var repository = GetRepository(session);
                    var option = repository.AsQueryable<Option>().FirstOrDefault();
                    if (option == null)
                    {
                        option = new Option();
                    }
                    option.DefaultMasterPage = null;
                    option.DefaultLayout = defaultLayout;
                    session.SaveOrUpdate(defaultLayout);

                    Run(session, api.Blog.BlogPost.Properties.Post, api.Blog.BlogPost.Properties.Get, api.Blog.BlogPost.Properties.Put, api.Blog.BlogPost.Properties.Delete);
                });

            // Detach from events
            Events.BlogEvents.Instance.BlogCreated -= Instance_EntityCreated;
            Events.BlogEvents.Instance.BlogUpdated -= Instance_EntityUpdated;
            Events.BlogEvents.Instance.BlogDeleted -= Instance_EntityDeleted;
        }

        protected override SaveBlogPostPropertiesModel GetCreateModel(ISession session)
        {
            var blogPost = TestDataProvider.CreateNewBlogPost();

            var widget = TestDataProvider.CreateNewHtmlContentWidget();
            session.SaveOrUpdate(widget);

            var assignmentId1 = Guid.NewGuid();
            var assignmentId2 = Guid.NewGuid();
            var html = string.Format("{0}{1}{3}{2}",
                TestDataProvider.ProvideRandomString(50),
                TestDataProvider.CreateChildWidgetAssignment(widget.Id, assignmentId1),
                TestDataProvider.ProvideRandomString(50),
                TestDataProvider.CreateChildWidgetAssignment(widget.Id, assignmentId2));
            
            session.SaveOrUpdate(blogPost.Category);
            session.SaveOrUpdate(blogPost.Author);
            session.SaveOrUpdate(blogPost.Image);
            session.SaveOrUpdate(blogPost.FeaturedImage);
            session.SaveOrUpdate(blogPost.SecondaryImage);

            session.Flush();
            session.Clear();

            var model = new SaveBlogPostPropertiesModel
                {
                    BlogPostUrl = string.Format("/{0}/", TestDataProvider.ProvideRandomString(200)),
                    Title = blogPost.Title,
                    IntroText = blogPost.Description,
                    IsPublished = true,
                    PublishedOn = blogPost.PublishedOn,
                    LayoutId = layout != null ? layout.Id : (defaultLayout != null ? defaultLayout.Id : (Guid?)null),
                    MasterPageId = masterPage != null ? masterPage.Id : (Guid?)null,
                    CategoryId = blogPost.Category.Id,
                    AuthorId = blogPost.Author.Id,
                    MainImageId = blogPost.Image.Id,
                    FeaturedImageId = blogPost.FeaturedImage.Id,
                    SecondaryImageId = blogPost.SecondaryImage.Id,
                    ActivationDate = blogPost.ActivationDate,
                    ExpirationDate = blogPost.ExpirationDate,
                    IsArchived = true,
                    UseCanonicalUrl = true,
                    UseNoFollow = true,
                    UseNoIndex = true,
                    HtmlContent = html,
                    Tags = new List<string> { TestDataProvider.ProvideRandomString(20), TestDataProvider.ProvideRandomString(20) },
                    MetaData = new MetadataModel
                               {
                                   MetaDescription = blogPost.MetaDescription,
                                   MetaKeywords = blogPost.MetaKeywords,
                                   MetaTitle = blogPost.MetaTitle
                               },
                    AccessRules = new List<AccessRuleModel>
                                  {
                                      new AccessRuleModel {
                                          AccessLevel = AccessLevel.ReadWrite, 
                                          Identity = TestDataProvider.ProvideRandomString(20),
                                          IsForRole = false
                                      },
                                      new AccessRuleModel {
                                          AccessLevel = AccessLevel.Deny, 
                                          Identity = TestDataProvider.ProvideRandomString(20),
                                          IsForRole = true
                                      }
                                  },
                    TechnicalInfo = new TechnicalInfoModel
                                {
                                      BlogPostContentId = Guid.NewGuid(),
                                      PageContentId = Guid.NewGuid(),
                                      RegionId = region.Id
                                },
                    ChildContentsOptionValues = new List<ChildContentOptionValuesModel>
                                {
                                    new ChildContentOptionValuesModel
                                    {
                                        AssignmentIdentifier = assignmentId1,
                                        OptionValues = new List<OptionValueModel>
                                        {
                                            new OptionValueModel
                                            {
                                                Key = "O1",
                                                Value = "V1",
                                                UseDefaultValue = false,
                                                Type = OptionType.Text
                                            }
                                        }
                                    },
                                    new ChildContentOptionValuesModel
                                    {
                                        AssignmentIdentifier = assignmentId2,
                                        OptionValues = new List<OptionValueModel>
                                        {
                                            new OptionValueModel
                                            {
                                                Key = "O2",
                                                Value = Guid.NewGuid().ToString(),
                                                UseDefaultValue = false,
                                                Type = OptionType.Custom,
                                                CustomTypeIdentifier = "media-images-folder"
                                            },
                                            new OptionValueModel
                                            {
                                                Key = "O3",
                                                Value = Guid.NewGuid().ToString(),
                                                UseDefaultValue = true,
                                                Type = OptionType.Text
                                            }
                                        }
                                    }
                                } 
                };

            return model;
        }

        protected override GetBlogPostPropertiesRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            var request = new GetBlogPostPropertiesRequest { BlogPostId = saveResponseBase.Data.Value };
            request.Data.IncludeHtmlContent = true;
            request.Data.IncludeTechnicalInfo = true;
            request.Data.IncludeAccessRules = true;
            request.Data.IncludeMetaData = true;
            request.Data.IncludeTags = true;
            request.Data.IncludeChildContentsOptions = true;

            return request;
        }

        protected override PutBlogPostPropertiesRequest GetUpdateRequest(GetBlogPostPropertiesResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Title = TestDataProvider.ProvideRandomString(MaxLength.Name);

            return request;
        }

        protected override void OnAfterGet(GetBlogPostPropertiesResponse getResponse, SaveBlogPostPropertiesModel model)
        {
            // Check if properties are not null
            Assert.IsNotNull(getResponse.Data.BlogPostUrl);
            Assert.IsNotNull(getResponse.Data.Title);
            Assert.IsNotNull(getResponse.Data.IntroText);
            Assert.IsNotNull(getResponse.Data.PublishedOn);
            Assert.IsNotNull(getResponse.Data.CategoryId);
            Assert.IsNotNull(getResponse.Data.MainImageId);
            Assert.IsNotNull(getResponse.Data.FeaturedImageId);
            Assert.IsNotNull(getResponse.Data.SecondaryImageId);
            Assert.IsNotNull(getResponse.Data.ActivationDate);
            Assert.IsNotNull(getResponse.Data.ExpirationDate);
            Assert.IsNotNull(getResponse.HtmlContent);
            Assert.IsNotNull(getResponse.Tags);
            Assert.IsNotNull(getResponse.MetaData);
            Assert.IsNotNull(getResponse.MetaData.MetaDescription);
            Assert.IsNotNull(getResponse.MetaData.MetaKeywords);
            Assert.IsNotNull(getResponse.MetaData.MetaTitle);
            Assert.IsNotNull(getResponse.TechnicalInfo);
            Assert.IsNotNull(getResponse.TechnicalInfo.BlogPostContentId);
            Assert.IsNotNull(getResponse.TechnicalInfo.RegionId);
            Assert.IsNotNull(getResponse.TechnicalInfo.PageContentId);
            Assert.IsNotNull(getResponse.AccessRules);
            Assert.IsNotNull(getResponse.ChildContentsOptionValues);
            Assert.IsNotEmpty(getResponse.ChildContentsOptionValues);
            if (masterPage != null)
            {
                Assert.IsNotNull(getResponse.Data.MasterPageId);
                Assert.IsNull(getResponse.Data.LayoutId);
            }
            else if (layout != null)
            {
                Assert.IsNotNull(getResponse.Data.LayoutId);
                Assert.IsNull(getResponse.Data.MasterPageId);
            }
            else
            {
                Assert.IsNotNull(getResponse.Data.MasterPageId ?? getResponse.Data.LayoutId);
            }

            // Compare saving entity with retrieved after save entity
            Assert.AreEqual(getResponse.Data.BlogPostUrl, model.BlogPostUrl);
            Assert.AreEqual(getResponse.Data.Title, model.Title);
            Assert.AreEqual(getResponse.Data.IntroText, model.IntroText);
            Assert.AreEqual(getResponse.Data.IsPublished, model.IsPublished);
            Assert.AreEqual(getResponse.Data.PublishedOn, model.PublishedOn);
            Assert.AreEqual(getResponse.Data.LayoutId, model.LayoutId);
            Assert.AreEqual(getResponse.Data.MasterPageId, model.MasterPageId);
            Assert.AreEqual(getResponse.Data.CategoryId, model.CategoryId);
            Assert.AreEqual(getResponse.Data.AuthorId, model.AuthorId);
            Assert.AreEqual(getResponse.Data.MainImageId, model.MainImageId);
            Assert.AreEqual(getResponse.Data.FeaturedImageId, model.FeaturedImageId);
            Assert.AreEqual(getResponse.Data.SecondaryImageId, model.SecondaryImageId);
            Assert.AreEqual(getResponse.Data.ActivationDate, model.ActivationDate);
            Assert.AreEqual(getResponse.Data.ExpirationDate, model.ExpirationDate);
            Assert.AreEqual(getResponse.Data.IsArchived, model.IsArchived);
            Assert.AreEqual(getResponse.Data.UseCanonicalUrl, model.UseCanonicalUrl);
            Assert.AreEqual(getResponse.Data.UseNoFollow, model.UseNoFollow);
            Assert.AreEqual(getResponse.Data.UseNoIndex, model.UseNoIndex);
            Assert.AreEqual(getResponse.HtmlContent, model.HtmlContent);

            Assert.AreEqual(getResponse.Tags.Count, model.Tags.Count);
            Assert.IsTrue(getResponse.Tags.All(t1 => model.Tags.Any(t2 => t2 == t1.Name)));

            Assert.AreEqual(getResponse.AccessRules.Count, model.AccessRules.Count);
            Assert.IsTrue(getResponse.AccessRules.All(a1 => model.AccessRules.Any(a2 => a1.AccessLevel == a2.AccessLevel 
                && a1.IsForRole == a2.IsForRole
                && a1.Identity == a2.Identity)));

            Assert.AreEqual(getResponse.MetaData.MetaKeywords, model.MetaData.MetaKeywords);
            Assert.AreEqual(getResponse.MetaData.MetaDescription, model.MetaData.MetaDescription);
            Assert.AreEqual(getResponse.MetaData.MetaTitle, model.MetaData.MetaTitle);
            
            Assert.AreEqual(getResponse.TechnicalInfo.BlogPostContentId, model.TechnicalInfo.BlogPostContentId);
            Assert.AreEqual(getResponse.TechnicalInfo.PageContentId, model.TechnicalInfo.PageContentId);
            Assert.AreEqual(getResponse.TechnicalInfo.RegionId, model.TechnicalInfo.RegionId);

            Assert.AreEqual(getResponse.ChildContentsOptionValues.Count, model.ChildContentsOptionValues.Count);
            model.ChildContentsOptionValues.ToList().ForEach(
                o =>
                {
                    var o1 = getResponse.ChildContentsOptionValues.FirstOrDefault(c => c.AssignmentIdentifier == o.AssignmentIdentifier);
                    Assert.IsNotNull(o1);
                    Assert.IsNotNull(o1.OptionValues);
                    Assert.AreEqual(o1.OptionValues.Count(c => !c.UseDefaultValue), o.OptionValues.Count(c => !c.UseDefaultValue));
                    Assert.IsTrue(o.OptionValues
                        .Where(c => !c.UseDefaultValue)
                        .All(c => o1.OptionValues.All(c1 => c1.Key == c.Key
                            && c.Value == c1.Value
                            && c.CustomTypeIdentifier == c1.CustomTypeIdentifier
                            && c.Type == c1.Type)));
                });
        }
    }
}
