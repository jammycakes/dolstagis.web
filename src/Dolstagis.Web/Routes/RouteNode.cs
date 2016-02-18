using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Routes
{
    public class RouteNode : Trie.Node<RouteNode, IRouteTarget>
    {
        private bool _isParameterParsed = false;
        private bool _isParameter = false;
        private string _parameterName = null;
        private bool _greedy = false;
        private bool _optional = false;

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void ParseParameter()
        {
            if (_isParameterParsed) return;
            _isParameterParsed = true;
            if (Name == null) return;
            _isParameter = Name.StartsWith("{") && Name.EndsWith("}");
            if (_isParameter) {
                string name = Name.Substring(1, Name.Length - 2);
                _greedy = name.EndsWith("+") | name.EndsWith("*");
                _optional = name.EndsWith("?") | name.EndsWith("*");
                if (_greedy || _optional)
                    name = name.Substring(0, name.Length - 1);
                _parameterName = name;
            }
        }

        public override bool IsParameter
        {
            get {
                ParseParameter();
                return _isParameter;
            }
        }

        public override string ParameterName
        {
            get {
                ParseParameter();
                return _parameterName;
            }
        }

        public override bool Greedy
        {
            get { return _greedy; }
        }

        public override bool Optional
        {
            get { return _optional; }
        }

        protected override void Validate()
        {
            if (Parent != null && Parent.IsParameter) {
                if (Parent.Greedy) {
                    throw new RouteException
                        ("Greedy route parameters must come last.");
                }
                if (Parent.Optional && !Optional) {
                    throw new RouteException
                        ("Optional route parameters may only be followed by other optional parameters.");
                }
            }
        }
    }
}
