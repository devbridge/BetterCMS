using System;
using System.Collections.Generic;
using System.Security.Principal;

using BetterCms.Core.Models;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;
using BetterCms.Module.Api.Operations.Root;
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
        private Page masterPage;

        /// <summary>
        /// TODO: TEMPORARY HACK - REMOVE THIS WHEN POSSIBLE
        /// </summary>
        private void SetIdentity()
        {
            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("John Doe"), RootModuleConstants.UserRoles.AllRoles);
        }

        [Test]
        public void Should_CRUD_BlogPost_WithLayout_Successfully()
        {
            SetIdentity();
            RunApiActionInTransaction(
                (api, session) =>
                {
                    masterPage = null;
                    layout = TestDataProvider.CreateNewLayout();

                    session.SaveOrUpdate(layout);

                    Run(session, api.Blog.BlogPost.Properties.Post, api.Blog.BlogPost.Properties.Get, api.Blog.BlogPost.Properties.Put, api.Blog.BlogPost.Properties.Delete);
                });
        }
        
        [Test]
        public void Should_CRUD_BlogPost_WithMasterPage_Successfully()
        {
            SetIdentity();
            RunApiActionInTransaction(
                (api, session) =>
                {
                    masterPage = TestDataProvider.CreateNewPage();
                    masterPage.IsMasterPage = true;
                    layout = null;

                    session.SaveOrUpdate(masterPage);

                    Run(session, api.Blog.BlogPost.Properties.Post, api.Blog.BlogPost.Properties.Get, api.Blog.BlogPost.Properties.Put, api.Blog.BlogPost.Properties.Delete);
                });
        }
        
        [Test]
        public void Should_CRUD_BlogPost_WithNoLayoutSpecified_Successfully()
        {
            SetIdentity();
            RunApiActionInTransaction(
                (api, session) =>
                {
                    masterPage = null;
                    layout = null;

                    Run(session, api.Blog.BlogPost.Properties.Post, api.Blog.BlogPost.Properties.Get, api.Blog.BlogPost.Properties.Put, api.Blog.BlogPost.Properties.Delete);
                });
        }

        protected override SaveBlogPostPropertiesModel GetCreateModel(ISession session)
        {
            var blogPost = TestDataProvider.CreateNewBlogPost();
            
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
                    LayoutId = layout != null ? layout.Id : (Guid?)null,
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
                    HtmlContent = TestDataProvider.ProvideRandomString(200),
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
                                  }
                };

            // TODO: TechnicalInfo

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
            Assert.IsNotNull(getResponse.Data.BlogPostUrl);

            Assert.AreEqual(getResponse.Data.BlogPostUrl, model.BlogPostUrl);
        }
    }
}
