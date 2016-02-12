using System;
using System.Collections;

namespace Dolstagis.Web.IoC
{
    public interface IServiceLocator
    {
        object Get(Type t);

        IEnumerable GetAll(Type t);
    }
}
