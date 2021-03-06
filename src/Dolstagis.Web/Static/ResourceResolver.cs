﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dolstagis.Web.Views;

namespace Dolstagis.Web.Static
{
    [Obsolete("Deprecate this in favour of ViewTable", true)]
    public class ResourceResolver : IResourceResolver
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
