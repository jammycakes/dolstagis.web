using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

        public Type HandlerType { get; set; }

        public MethodInfo Method { get; set; }

        public object[] Arguments { get; set; }

        public object Invoke(IRequestContext context)
        {
            var instance = _container.GetInstance(HandlerType) as Handler;
            instance.Context = context;
            return Method.Invoke(instance, Arguments.ToArray());
        }
    }
}
