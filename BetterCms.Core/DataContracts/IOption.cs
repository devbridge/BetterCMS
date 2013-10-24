using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Core.DataContracts
{
    /// <summary>
    /// Defines interface to access the option key/type/value.
    /// </summary>
    public interface IOption : IEntity
    {
        string Key { get; set; }

        OptionType Type { get; set; }

        string Value { get; set; }

        ICustomOption CustomOption { get; set; }
    }
}
