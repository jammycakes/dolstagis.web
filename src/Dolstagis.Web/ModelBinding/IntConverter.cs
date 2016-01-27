namespace Dolstagis.Web.ModelBinding
{
    public class IntConverter : SimpleConverter<int>
    {
        protected override object Parse(string s)
        {
            return int.Parse(s);
        }
    }
}
