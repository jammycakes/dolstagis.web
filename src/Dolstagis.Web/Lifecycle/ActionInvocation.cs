using System;
using System.Reflection;

namespace Dolstagis.Web.Lifecycle
{
    public class ActionInvocation
    {
        public Type ControllerType { get; set; }

        public MethodInfo Method { get; set; }

        public object[] Arguments { get; set; }
    }
}
