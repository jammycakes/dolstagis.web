using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Static
{
    public class ResourceLocator
    {
        private IEnumerable<ResourceLocation> _locations;

        public ResourceLocator(string type, IEnumerable<ResourceLocation> locations)
        {
            _locations = locations.Where(x => x.Type == type).ToList(); ;
        }

        public IResource GetResource(VirtualPath path)
        {
            var candidates =
                from location in _locations
                let mine = location.Root.GetSubPath(path, true)
                where mine != null
                orderby location.Root.Parts.Count descending
                let resource = location.GetResource(mine)
                where resource != null
                select resource;

            return candidates.FirstOrDefault();
        }
    }
}
