using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Core.DataContracts
{
    /// <summary>
    /// Defines interface to access the option key/type/casted value.
    /// </summary>
    public interface IOptionValue : IOption
    {
        bool UseDefaultValue { get; set; }
    }
}
