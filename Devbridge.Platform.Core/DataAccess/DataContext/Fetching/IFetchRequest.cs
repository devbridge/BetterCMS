using System.Linq;

namespace Devbridge.Platform.Core.DataAccess.DataContext.Fetching
{
    public interface IFetchRequest<TQueried, TFetch> : IOrderedQueryable<TQueried>
    {
    }
}
