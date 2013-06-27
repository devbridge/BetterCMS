using System;

using BetterCms.Module.Api.Operations.Root.GetTagByName;
using BetterCms.Module.Api.Operations.Root.GetTags;
using BetterCms.Module.Api.Operations.Root.Version;

namespace BetterCms.Module.Api.Operations.Root
{
    public class DefaultRootOperationsContext : IRootOperationsContext
    {
        
        public GetVersionResponse GetVersion()
        {
            throw new NotImplementedException();
        }

        public GetTagsResponse GetTags(GetTagsRequest request)
        {
            throw new NotImplementedException();
        }

        public GetTagByNameResponse GetTagByName(GetTagByNameRequest request)
        {
            throw new NotImplementedException();
        }
    }
}