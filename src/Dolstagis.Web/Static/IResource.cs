using System;
using System.Collections.Generic;
using System.IO;

namespace Dolstagis.Web.Static
{
    public interface IResource
    {
        bool IsFile { get; }

        DateTime LastModified { get; }

        long? Length { get; }

        string Name { get; }

        Stream Open();
    }
}
