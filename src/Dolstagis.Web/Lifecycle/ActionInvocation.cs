using System;
using System.Linq;
using System.Reflection;
using StructureMap;

namespace Dolstagis.Web.Lifecycle
{
    public class ActionInvocation
    {
        private IContainer _container;

        public ActionInvocation(IContainer container)
        {
            _container = container;
        }

        public Type ControllerType { get; set; }

        public MethodInfo Method { get; set; }

        public object[] Arguments { get; set; }

        public object Invoke(RequestContext context)
        {
            var instance = _container.GetInstance(ControllerType) as Controller;
            instance.Context = context;
            return Method.Invoke(instance, Arguments.ToArray());
        }
    }
}
