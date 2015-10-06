using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts
{
    [DataContract]
    [Serializable]
    public class GetBlogPostsRequest : RequestBase<GetBlogPostsModel>, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Data.HasColumnInSortBySection("Tags") || Data.HasColumnInWhereSection("Tags"))
            {
                yield return new ValidationResult("An Tags field is a list. You can't sort or add filter by this column.", new List<string> {"Data"});
            }        
        }
    }
}