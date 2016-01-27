using System.Collections.Generic;
using System.Text;

namespace Dolstagis.Web
{
    public abstract class ResultBase
    {
        protected string GetHeader(string key)
        {
            string result;
            return Headers.TryGetValue(key, out result) ? result : null;
        }

        protected void SetHeader(string key, string value)
        {
            if (key == null)
            {
                if (Headers.ContainsKey(key))
                {
                    Headers.Remove(key);
                }
            }
            else
            {
                Headers[key] = value;
            }
        }

        public Status Status { get; set; }

        public Encoding Encoding { get; set; }

        public string ContentType
        {
            get { return GetHeader("Content-Type"); }
            set { SetHeader("Content-Type", value); }
        }

        public IDictionary<string, string> Headers { get; private set; }

        public ResultBase()
        {
            Headers = new Dictionary<string, string>();
            Status = Status.OK;
            Encoding = Encoding.UTF8;
        }
    }
}
