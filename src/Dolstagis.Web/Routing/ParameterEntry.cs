﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dolstagis.Web.Routing
{
    public class ParameterEntry : RouteTableEntry
    {
        public ParameterEntry(IRouteDefinition definition, string name)
            : base(definition, name)
        {
            if (!name.StartsWith("{") || !name.EndsWith("}")) {
                throw new ArgumentException("Parameter names must be enclosed in braces", "name");
            }
            string s = name.Substring(1, name.Length - 2);
            char last = s.LastOrDefault();
            Greedy = "*+".Contains(last);
            Optional = "*?".Contains(last);
            if (Greedy || Optional) {
                s = s.Substring(s.Length - 1);
            }
            ParameterName = s;
        }

        public override RouteTableEntry GetOrCreateChild(string name)
        {
            if (this.Greedy) {
                throw new InvalidOperationException("Greedy or greedy-optional parameters must come last.");
            }

            if (this.Optional && !Regex.IsMatch(name, @"^\{.*[\?\*]\}$")) {
                throw new InvalidOperationException("Optional parameters can only be followed by other optional parameters.");
            }
            return base.GetOrCreateChild(name);
        }

        public string ParameterName { get; private set; }

        public bool Greedy { get; private set; }

        public bool Optional { get; private set; }
    }
}
