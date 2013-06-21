using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Blog.Api.DataModels;

namespace BetterCms.Module.Blog.Api.DataContracts
{
    public class GetBlogPostsRequest : GetDataRequest<BlogPostModel>
    {
        public GetBlogPostsRequest()
        {
        }

        public GetBlogPostsRequest(Expression<Func<BlogPostModel, bool>> filter = null, 
            Expression<Func<BlogPostModel, dynamic>> order = null, 
            bool orderDescending = false, 
            bool includeUnpublished = false,
            bool includeNotActive = false,
            int? itemsCount = null, 
            int startItemNumber = 1,
            string[] tags = null)
            : base(filter, order, orderDescending, itemsCount, startItemNumber)
        {
            IncludeUnpublished = includeUnpublished;
            IncludeNotActive = includeNotActive;
            Tags = tags;
        }

        public string[] Tags { get; set; }

        public bool IncludeUnpublished { get; set; }

        public bool IncludeNotActive { get; set; }
    }
}