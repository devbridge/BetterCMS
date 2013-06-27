using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tag
{
    [Route("/tag", Verbs = "POST")]   
    public class PostTagRequest : TagModel, IReturn<PostTagResponse>
    {        
    }
}