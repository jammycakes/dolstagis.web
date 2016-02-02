namespace Dolstagis.Web.ModelBinding
{
    public class StringConverter : SimpleConverter<string>
    {
        protected override object Parse(string s)
        {
            return s;
        }
    }
}
