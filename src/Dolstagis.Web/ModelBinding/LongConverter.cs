namespace Dolstagis.Web.ModelBinding
{
    public class LongConverter : SimpleConverter<long>
    {
        protected override object Parse(string s)
        {
            return long.Parse(s);
        }
    }
}
