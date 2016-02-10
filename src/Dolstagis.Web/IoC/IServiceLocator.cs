using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.IoC
{
    public interface IServiceLocator
    {
        object Get(Type t);

        IEnumerable GetAll(Type t);
    }
}
