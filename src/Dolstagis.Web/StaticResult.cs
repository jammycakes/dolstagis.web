using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Routing;

namespace Dolstagis.Web
{
    public class StaticResult
    {
        public string Path { get; private set; }

        public StaticResult(string path)
        {
            this.Path = path.NormaliseUrlPath();
        }
    }
}
