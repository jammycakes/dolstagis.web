using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.Static;
using Dolstagis.Web.Views;

namespace Dolstagis.Web
{
    public class Controller
    {
        public RequestContext Context { get; internal set; }

        /// <summary>
        ///  Returns a static content result.
        /// </summary>
        /// <param name="path">
        ///  The path to the static content.
        /// </param>
        /// <returns></returns>
        public ResultBase Static(string path)
        {
            return new StaticResult(new VirtualPath(path));
        }

        /// <summary>
        ///  Returns a static content result.
        /// </summary>
        /// <param name="path">
        ///  The path to the static content.
        /// </param>
        /// <returns></returns>
        public ResultBase Static(VirtualPath path)
        {
            return new StaticResult(path);
        }

        /// <summary>
        ///  Returns a view result.
        /// </summary>
        /// <param name="path">
        ///  The path to the view template.
        /// </param>
        /// <returns></returns>
        public ResultBase View(string path)
        {
            return new ViewResult(path);
        }

        /// <summary>
        ///  Returns a view result with a model.
        /// </summary>
        /// <param name="path">
        ///  The path to the view result.
        /// </param>
        /// <param name="model">
        ///  The model.
        /// </param>
        /// <returns></returns>
        public ResultBase View(string path, object model)
        {
            return new ViewResult(path, model);
        }

        /// <summary>
        ///  Returns a JSON result
        /// </summary>
        /// <param name="data">
        ///  The object to serialise as JSON.
        /// </param>
        /// <returns></returns>
        public ResultBase Json(object data)
        {
            return new JsonResult(data);
        }

        /// <summary>
        ///  Returns a content result of content type text/plain.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public ContentResult Content(string content)
        {
            return new ContentResult(content);
        }

        /// <summary>
        ///  Returns a content result with a given content type.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public ContentResult Content(string content, string contentType)
        {
            return new ContentResult(content, contentType);
        }

        /// <summary>
        ///  Returns a temporary redirection to an alternative resource.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public RedirectResult Redirect(string url)
        {
            return new RedirectResult(url);
        }

        /// <summary>
        ///  Returns a permanent redirection to an alternative resource.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public RedirectResult RedirectPermanently(string url)
        {
            return new RedirectResult(url, Status.MovedPermanently);
        }
    }
}
