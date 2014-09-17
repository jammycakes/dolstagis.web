About Dolstagis.Web
===================

Dolstagis.Web is an ALT.MVC web framework, intended as a lightweight alternative to ASP.NET MVC and
WebAPI, and similar in intent to frameworks such as
[NancyFX](http://nancyfx.org/), [FubuMVC](http://mvc.fubu-project.org) and
[Simple.Web](https://github.com/markrendle/simple.web).

The main design goal of Dolstagis.Web is modularity: the features that make up a Dolstagis.Web
application can be switched in and out as needed. This will allow us to support architectures such
as feature toggles, A/B testing, go-live dates and degradation under load.

Quick start
-----------
Create a new empty web project then add references to the following packages:

 * Dolstagis.Web
 * Dolstagis.Web.Aspnet
 * Dolstagis.Web.Nustache

Add a file called DolstagisConfiguration.cs to the web project:

```c#
using Dolstagis.Web.Aspnet;

namespace WebApp
{
    public class DolstagisConfiguration : IConfigurator
    {
        public void Configure(Dolstagis.Web.Application application)
        {
            application.ScanForFeatures();
        }
    }
}
```

Add a handler to your project, this is derived from `Dolstagis.Web.Handler` and performs a role
similar to classic MVC controllers:

```c#
using Dolstagis.Web;

namespace WebApp
{
    [Route("/")]
    public class Index : Handler
    {
        public object Get()
        {
            return View("~/hello.nustache", new { Message = "Hello from Nustache" });
        }
    }
}
```

Add a feature to your project, this is derived from `Dolstagis.Web.Feature` and defines the
handlers, static files and view template locations that it uses:

```c#
using Dolstagis.Web;

namespace WebApp
{
    public class HomeFeature : Feature
    {
        public HomeFeature()
        {
            AddStaticFiles("~/content");
            AddViews("~/", "views");
            AddHandler<Index>();
        }
    }
}
```

In this example, static files will be served up from a folder called "content" and views will be
placed in a folder called "views". Dolstagis.Web uses Nustache as its view engine; support for
other view engines is planned.

Each feature includes a StructureMap `Registry` where you can declare your dependencies for
injection. See the source code for examples on how to do this. Dolstagis.Web also uses StructureMap
to create your handlers, so you can inject dependencies into these as required.

Current status
--------------
This version is still in pre-alpha stage and is not yet in use on any serious projects: I plan to
start dogfooding it after the version 0.3 release.

At present the following features have been implemented:

 * Features, handlers and a routing engine
 * Feature switches
 * Nustache-based Views, JSON and static files
 * Session state
 * A rudimentary authentication API

The API is still unstable and subject to change between releases.
