namespace BetterCms.Api.Interfaces.Models
{
    /// <summary>
    /// Defines interface to access the content option.
    /// </summary>
    public interface IContentOption : IOption
    {
        IContent Content { get; }        
    }
}
