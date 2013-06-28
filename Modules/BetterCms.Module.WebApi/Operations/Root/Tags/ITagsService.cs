using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    public interface ITagsService
    {
        GetTagsResponse Get(GetTagsRequest request);
    }
}
