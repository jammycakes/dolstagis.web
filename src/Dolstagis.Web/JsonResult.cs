using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    public class JsonResult : ResultBase
    {
        public object Data { get; set; }

        public JsonResult(object data)
        {
            Data = data;
        }
    }
}
