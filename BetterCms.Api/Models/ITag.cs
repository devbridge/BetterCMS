using BetterCms.Core.Models;

namespace BetterCms.Api.Models
{
    public interface ITag : IEntity
    {
        string Name { get; }
    }
}
