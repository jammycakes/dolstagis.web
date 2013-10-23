using Dolstagis.Web.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Views;

namespace Dolstagis.Web
{
    public class Handler
    {
        public IRequestContext Context { get; internal set; }

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
    }
}
