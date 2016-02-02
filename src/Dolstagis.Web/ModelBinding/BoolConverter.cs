namespace Dolstagis.Web.ModelBinding
{
    public class BoolConverter : SimpleConverter<bool>
    {
        protected override object Parse(string s)
        {
            return bool.Parse(s);
        }
    }
}
