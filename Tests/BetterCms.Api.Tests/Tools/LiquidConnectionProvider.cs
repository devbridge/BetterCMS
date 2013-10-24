using System.Web.Hosting;

using NHibernate.Connection;

namespace BetterCms.Api.Tests.Tools
{
    public class LiquidConnectionProvider : DriverConnectionProvider
    {
        internal static DatabaseRefresher DatabaseRefresher;

        public LiquidConnectionProvider()
        {
            DatabaseRefresher = new DatabaseRefresher(HostingEnvironment.MapPath("~/App_Data"), HostingEnvironment.MapPath("~/App_Data/Temp"));
            DatabaseRefresher.BindNewDatabase();
        }

        protected override string ConnectionString
        {
            get
            {
                return DatabaseRefresher.CurrentConnectionString;
            }
        }
        
    }
}