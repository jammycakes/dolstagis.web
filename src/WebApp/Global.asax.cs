using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Dolstagis.Web.Aspnet;

namespace WebApp
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Startup.ConfigureApplication(x => x.AddAllFeaturesInAssemblyOf<Global>());
        }
    }
}