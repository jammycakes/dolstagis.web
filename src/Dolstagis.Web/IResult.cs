using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    public interface IResult
    {
        Task RenderAsync(IRequestContext context);

        Status Status { get; set; }
    }
}
