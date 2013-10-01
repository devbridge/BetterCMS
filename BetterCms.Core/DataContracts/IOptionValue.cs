using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Core.DataContracts
{
    /// <summary>
    /// Defines interface to access the option key/type/casted value.
    /// </summary>
    public interface IOptionValue
    {
        string Key { get; }

        OptionType Type { get; }

        object Value { get; }

        ICustomOption CustomOption { get; set; }
    }
}
