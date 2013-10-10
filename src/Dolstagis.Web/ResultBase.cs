using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    public abstract class ResultBase
    {
        public Status Status { get; set; }

        public IDictionary<string, string> Headers { get; private set; }

        public ResultBase()
        {
            Headers = new Dictionary<string, string>();
            Status = Status.OK;
        }
    }
}
