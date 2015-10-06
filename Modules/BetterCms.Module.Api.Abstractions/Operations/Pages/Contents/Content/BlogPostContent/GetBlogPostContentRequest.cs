using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent
{
    [DataContract]
    [System.Serializable]
    public class GetBlogPostContentRequest 
    {
        [DataMember]
        public System.Guid ContentId
        {
            get; set;
        }
    }
}