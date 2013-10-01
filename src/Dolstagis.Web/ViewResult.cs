using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    public class ViewResult
    {
        public string Path { get; private set; }

        public object Data { get; private set; }

        public ViewResult(string path)
        {
            this.Path = path;
            this.Data = null;
        }

        public ViewResult(string path, object data)
        {
            this.Path = path;
            this.Data = data;
        }
    }
}
