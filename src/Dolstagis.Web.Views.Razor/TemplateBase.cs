using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Views.Razor
{
    public abstract class TemplateBase
    {
        private StringBuilder _buffer = new StringBuilder();
        

        protected string Html(object value)
        {
            if (value == null) return String.Empty;
            if (value is IHtmlString) return ((IHtmlString)value).ToHtmlString();
            return HttpUtility.HtmlEncode(value.ToString());
        }

        #region /* ====== Methods required by view compiler ====== */

        public abstract void Execute();

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void Write(object value)
        {
            _buffer.Append(Html(value));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void WriteLiteral(object value)
        {
            _buffer.Append(value);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void WriteTo(TextWriter writer, object value)
        {
            writer.Write(Html(value));
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void WriteLiteralTo(TextWriter writer, object value)
        {
            writer.Write(value);
        }

        public virtual void DefineSection(string name, Action action)
        {
        }

        #endregion
    }
}
