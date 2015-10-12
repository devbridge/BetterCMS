using System.Collections.Generic;

using BetterCms.Core.DataContracts;

namespace BetterCms.Core
{
    public interface IMultilingualOption
    {
        IList<IOptionTranslation> Translations { get; set; }
    }
}
