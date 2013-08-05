using System.Collections.Generic;

namespace BetterCms.Core.DataContracts
{
    public interface IOptions
    {
        IEnumerable<IOption> Options { get; }
    }
}
