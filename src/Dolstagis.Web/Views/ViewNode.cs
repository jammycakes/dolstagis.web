using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Routes.Trie;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views
{
    public class ViewNode : Node<ViewNode, ViewRegistration>
    {
        public override bool IsParameter
        {
            get { return Name == "*"; }
        }

        public override bool Greedy
        {
            get { return IsParameter; }
        }

        public override bool Optional
        {
            get { return IsParameter;  }
        }

        public override string ParameterName
        {
            get { return IsParameter ? "path" : null; }
        }
    }
}
