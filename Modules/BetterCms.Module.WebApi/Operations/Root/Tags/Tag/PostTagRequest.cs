using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    [Route("/tag", Verbs = "POST")]   
    public class PostTagRequest : TagModel, IReturn<PostTagResponse>
    {        
    }
}