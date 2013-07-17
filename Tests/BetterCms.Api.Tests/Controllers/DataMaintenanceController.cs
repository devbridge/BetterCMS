using System;
using System.IO;
using System.Web.Mvc;

using BetterCms.Api.Tests.Tools;

namespace BetterCms.Api.Tests.Controllers
{
    public class DataMaintenanceController : Controller
    {
#if DEBUG
        [AllowAnonymous]
        [HttpGet]
        public ActionResult RegisterNewDataSet()
        {
            LiquidConnectionProvider.DatabaseRefresher.BindNewDatabase();

            return Redirect("/");
        }
#endif
    }
}