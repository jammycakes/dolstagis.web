using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Views.Razor
{
    public abstract class TemplateBase
    {

        #region /* ====== Methods required by view compiler ====== */

        public abstract void Execute();

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void Write(object value)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void WriteLiteral(object value)
        {
        }

        public virtual void DefineSection(string name, Action action)
        {
        }

        #endregion
    }
}
