using System;
using System.Linq;
using System.Reflection;

namespace Dolstagis.Web.Lifecycle
{
    public class ActionInvocation
    {
        private IIoCContainer _container;

        public ActionInvocation(IIoCContainer container)
        {
            _container = container;
        }

        public Type ControllerType { get; set; }

        public MethodInfo Method { get; set; }

        public object[] Arguments { get; set; }

        public object Invoke(RequestContext context)
        {
            var instance = _container.GetService(ControllerType) as Controller;
            instance.Context = context;
            return Method.Invoke(instance, Arguments.ToArray());
        }
    }
}
