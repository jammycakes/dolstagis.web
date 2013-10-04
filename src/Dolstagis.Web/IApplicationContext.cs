using System;

namespace Dolstagis.Web
{
    public interface IApplicationContext
    {
        string PhysicalPath { get; }
        string VirtualPath { get; }
    }
}
