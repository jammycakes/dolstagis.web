using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dolstagis.Web.Util;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Owin;

[assembly: OwinStartup(typeof(Dolstagis.Web.Aspnet.Startup))]

namespace Dolstagis.Web.Aspnet
{
    using MidFunc = Func<
            Func<IDictionary<string, object>, Task>,
            Func<IDictionary<string, object>, Task>
        >;

    public class Startup
    {
        /*
         * Adding this dummy reference to the OwinHttpHandler type ensures that
         * Microsoft.Owin.Host.SystemWeb will always be included in the build of
         * your web application, even if it isn't explicitly referenced by your
         * front end web project itself.
         * 
         * This is necessary to ensure that the Owin infrastructure gets loaded
         * by System.Web in the first place.
         */

        private static readonly object dummy = typeof(Microsoft.Owin.Host.SystemWeb.OwinHttpHandler);
        private static Action<Application> _configurer = app => app.ScanForFeatures();

        public static void ConfigureApplication(Action<Application> configurer)
        {
            _configurer = configurer;
        }


        public void Configuration(IAppBuilder app)
        {
            var application = new Application(new Settings());
            if (_configurer != null) _configurer(application);

            MidFunc middleware = next => application.GetAppFunc();
            app.Use(middleware);
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}
