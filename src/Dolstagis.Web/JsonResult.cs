namespace Dolstagis.Web
{
    public class JsonResult : ResultBase
    {
        public object Data { get; set; }

        public JsonResult(object data)
        {
            Data = data;
            ContentType = "application/json";
        }
    }
}
