using System.Collections.Generic;

namespace BetterCms.Core.DataContracts
{
    public interface IRenderPage : IPage
    {
        IList<IOptionValue> Options { get; }
    }
}
