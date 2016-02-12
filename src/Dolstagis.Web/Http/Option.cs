using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Http
{
    public class Option
    {
        public string Value { get; private set; }

        public float Q { get; private set; }

        public Option(string content)
        {
            var parts = content.Split(';').Select(x => x.Trim());
            Value = parts.First();
            string qv = parts.Skip(1).Where(x => x.StartsWith("q="))
                .Select(x => x.Substring(2)).FirstOrDefault();
            float q;
            Q = (float.TryParse(qv, out q)) ? q : 1;
        }

        public static IEnumerable<Option> ParseAll(string header)
        {
            return
                from s in header.Split(',')
                let result = new Option(s)
                orderby result.Q descending
                select result;
        }
    }
}
