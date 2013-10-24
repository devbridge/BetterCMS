using BetterCms.Module.Api.Infrastructure;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts
{
    public class GetBlogPostsRequestValidator : AbstractValidator<GetBlogPostsRequest>
    {
        public GetBlogPostsRequestValidator()
        {
            RuleFor(f => f.Data).Must(TagsListIsReadonly).WithMessage("An Tags field is a list. You can't sort or add filter by this column.");
        }

        private bool TagsListIsReadonly(GetBlogPostsRequest request, GetBlogPostsModel data)
        {
            return !data.HasColumnInSortBySection("Tags") && !data.HasColumnInWhereSection("Tags");            
        }
    }
}
