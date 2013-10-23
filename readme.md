About Dolstagis.Web
===================

Dolstagis.Web is an ALT.MVC web framework, intended as a lightweight alternative
to ASP.NET MVC and WebAPI, and similar in intent to frameworks such as
[NancyFX](http://nancyfx.org/), [FubuMVC](http://mvc.fubu-project.org) and
[Simple.Web](https://github.com/markrendle/simple.web).

The main design goal of Dolstagis.Web is modularity: the modules that make up a
Dolstagis.Web application can be switched in and out as needed. This will allow
us to support architectures such as feature toggles, A/B testing, go-live dates
and degradation under load.

Quick start
-----------
Create a new empty web project then add references to the following packages:

 * Dolstagis.Web
 * Dolstagis.Web.Aspnet
 * Dolstagis.Web.Nustache

Add the following section to your web.config:

```xml
<system.webServer>
	<handlers>
		<clear />
		<add name="Dolstagis" type="Dolstagis.Web.Aspnet.HttpRequestHandler, Dolstagis.Web.Aspnet" path="*" verb="*" />
	</handlers>
</system.webServer>
```

Add a handler to your project, this is derived from `Dolstagis.Web.Handler` and performs
a role similar to classic MVC controllers:

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

Add a module to your project, this is derived from `Dolstagis.Web.Module` and defines
the handlers, static files and view template locations that it uses:

```c#
using Dolstagis.Web;

namespace WebApp
{
	public class HomeModule : Module
	{
		public HomeModule()
		{
			AddStaticFiles("~/content");
			AddViews("~/", "views");
			AddHandler<Index>();
		}
	}
}
```

In this example, static files will be served up from a folder called "content"
and views will be placed in a folder called "views". Dolstagis.Web uses Nustache
as its view engine; support for other view engines is planned.

Each module includes a StructureMap `Registry` where you can declare your
dependencies for injection. See the source code for examples on how to do this.
Dolstagis.Web also uses StructureMap to create your handlers, so you can
inject dependencies into these as required.

Current status
--------------
The initial release is a "minimum viable" MVC framework. It will serve up views,
JSON and static files, but that's about it. The API is not stable and certain
behaviours (such as what happens when you have an ambiguous route match) are
at present undefined. Switching of modules has not yet been implemented.
