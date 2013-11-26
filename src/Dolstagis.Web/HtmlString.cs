using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    public class HtmlString : IHtmlString
    {
        private string _content;

        public HtmlString(string content)
        {
            _content = content;
        }

        public string ToHtmlString()
        {
            return _content;
        }

        public override string ToString()
        {
            return _content;
        }
    }
}
