using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Auth
{
    public interface IUser
    {
        string UserName { get; }

        bool IsInRole(string role);
    }
}
