using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dolstagis.Web;

namespace WebApp
{
    public class HomeModule : Module
    {
        public HomeModule()
        {
            AddStaticFiles("~/content");
            AddHandler<Index>();
            AddViews("~/", "views");
        }
    }
}