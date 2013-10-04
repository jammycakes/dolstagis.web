using System;

namespace Dolstagis.Web
{
    public interface IApplicationContext
    {
        string PhysicalPath { get; }
        VirtualPath VirtualPath { get; }
    }
}
