using System;

namespace Dolstagis.Web.ModelBinding
{
    public class GuidConverter : SimpleConverter<Guid>
    {
        protected override object Parse(string s)
        {
            return Guid.Parse(s);
        }
    }
}
