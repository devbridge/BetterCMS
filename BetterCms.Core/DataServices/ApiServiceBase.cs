using Common.Logging;

namespace BetterCms.Core.DataServices
{
    public abstract class ApiServiceBase
    {
        protected static readonly ILog Logger = LogManager.GetLogger("Api");
    }
}
