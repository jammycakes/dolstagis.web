using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    public class RedirectResult : HeadResult
    {
        public string Location
        {
            get { return GetHeader("Location"); }
            set { SetHeader("Location", value); }
        }

        public RedirectResult(string location, Status status = null)
        {
            this.Location = location;
            this.Status = status ?? Status.TemporaryRedirect;
        }
    }
}
