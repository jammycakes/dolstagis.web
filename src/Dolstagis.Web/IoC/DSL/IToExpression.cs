using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.IoC.DSL
{
    public interface IToExpression
    {
        void Transient();

        void Managed();
    }
}
