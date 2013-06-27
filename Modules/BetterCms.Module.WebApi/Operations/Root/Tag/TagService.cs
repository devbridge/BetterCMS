using System;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Tag
{
    public class TagService : Service
    {
        public GetTagResponse Get(GetTagRequest request)
        {
            return new GetTagResponse
                       {
                           Data = new TagModel
                                      {
                                          Id = request.TagId ?? Guid.NewGuid(),
                                          Name = request.TagName ?? "test",
                                          IsDeleted = false,
                                          Version = 5
                                      },
                           Status = "ok"
                       };
        }

        public PostTagResponse Post(PostTagRequest request)
        {
            return new PostTagResponse
            {
                Data = true,
                Status = "ok"
            };
        }    
    
        // TODO: add delete action
    }
}