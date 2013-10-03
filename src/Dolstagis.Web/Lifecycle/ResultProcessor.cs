using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Lifecycle
{
    public abstract class ResultProcessor<T> : IResultProcessor
    {
        public bool CanProcess(object data)
        {
            return (data is T);
        }

        public Task Process(object data, IRequestContext context)
        {
            return Process((T)data, context);
        }

        public abstract Task Process(T data, IRequestContext context);
    }
}
