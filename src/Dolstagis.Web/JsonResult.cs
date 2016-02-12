namespace Dolstagis.Web
{
    public class JsonResult : ResultBase
    {
        public object Data { get; set; }

        public JsonResult(object data)
        {
            Data = data;
            MimeType = "application/json";
            Encoding = System.Text.Encoding.UTF8;
        }
    }
}
