using System.Collections.Generic;

namespace BetterCms.Core.DataContracts
{
    public interface IOptionValues
    {
        IEnumerable<IOption> OptionValues { get; }
    }
}
