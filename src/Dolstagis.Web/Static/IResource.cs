using System;
using System.IO;

namespace Dolstagis.Web.Static
{
    public interface IResource
    {
        bool Exists { get; }

        DateTime LastModified { get; }

        long? Length { get; }

        string Name { get; }

        Stream Open();
    }
}
