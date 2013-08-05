using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Core.DataContracts
{
    /// <summary>
    /// Defines interface to access the option key/type/value.
    /// </summary>
    public interface IOption
    {
        string Key { get; }

        OptionType Type { get; }
        
        string Value { get; }    
    }
}
