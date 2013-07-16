using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterCms.Module.Api.Operations.Root.Layouts
{
    public interface ILayoutsService
    {
        GetLayoutsResponse Get(GetLayoutsRequest request);
    }
}
