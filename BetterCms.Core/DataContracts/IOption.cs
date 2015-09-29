using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Core.DataContracts
{
    public interface IOption
    {
        string Key { get; set; }

        OptionType Type { get; set; }

        string Value { get; set; }

        ICustomOption CustomOption { get; set; }
    }
}
