using BetterCms.Api.Interfaces.Models.Enums;

namespace BetterCms.Api.Interfaces.Models
{
    /// <summary>
    /// Defines interface to access the option key/type/value.
    /// </summary>
    public interface IOption : IEntity
    {
        string Key { get; }

        OptionType Type { get; }
        
        string Value { get; }    
    }
}
