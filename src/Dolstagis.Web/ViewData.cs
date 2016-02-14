using System.Collections.Generic;
using System.Text;

namespace Dolstagis.Web
{
    public class ViewData
    {
        public IDictionary<string, object> Data { get; set; }
        public Encoding Encoding { get; set; }
        public object Model { get; set; }
        public VirtualPath Path { get; set; }
        public Status Status { get; set; }
    }
}