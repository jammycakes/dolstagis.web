using System.Collections.Generic;
using System.Linq;

namespace Dolstagis.Web.Static
{
    public class ResourceResolver
    {
        private IEnumerable<ResourceMapping> _mappings;

        public ResourceResolver(IEnumerable<ResourceMapping> mappings)
        {
            _mappings = mappings.ToList();
        }

        public IResource GetResource(VirtualPath path)
        {
            var candidates =
                from mapping in _mappings
                let mine = mapping.Root.GetSubPath(path, true)
                where mine != null
                orderby mapping.Root.Parts.Count descending
                let resource = mapping.GetResource(mine)
                where resource != null
                select resource;

            return candidates.FirstOrDefault();
        }
    }
}
