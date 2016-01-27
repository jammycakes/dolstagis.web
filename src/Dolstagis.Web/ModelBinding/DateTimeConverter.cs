using System;

namespace Dolstagis.Web.ModelBinding
{
    public class DateTimeConverter : SimpleConverter<DateTime>
    {
        protected override object Parse(string s)
        {
            return DateTime.Parse(s);
        }
    }
}
