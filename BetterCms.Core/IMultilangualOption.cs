using System.Collections.Generic;

using BetterCms.Core.DataContracts;

namespace BetterCms.Core
{
    public interface IMultilangualOption
    {
        IList<IOptionTranslation> Translations { get; set; }
    }
}
