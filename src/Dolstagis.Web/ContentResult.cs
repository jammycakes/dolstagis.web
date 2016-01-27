namespace Dolstagis.Web
{
    public class ContentResult : ResultBase
    {
        public string Content { get; set; }

        public ContentResult(string content = "", string contentType = "text/plain") : base()
        {
            this.Content = content;
            this.ContentType = contentType;
        }
    }
}
