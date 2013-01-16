namespace BetterCms.Core.Models
{
    /// <summary>
    /// Defines interface to access the content option.
    /// </summary>
    public interface IContentOption : IEntity
    {
        IContent Content { get; }

        string Key { get; }

        ContentOptionType Type { get; }
        
        string DefaultValue { get; }    
    }
}
