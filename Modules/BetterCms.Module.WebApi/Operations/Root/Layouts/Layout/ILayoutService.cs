using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    public interface ILayoutService
    {
        GetLayoutResponse Get(GetLayoutRequest request);
    }
}
