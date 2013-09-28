using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    public class Module
    {
        public virtual string Description { get; }

        public bool Enabled { get; set; }

        public Module()
        {
            this.Enabled = true;
        }
    }
}
