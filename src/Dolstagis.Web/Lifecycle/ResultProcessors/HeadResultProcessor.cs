using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dolstagis.Web.Auth;
using Dolstagis.Web.Http;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Sessions;

namespace Dolstagis.Web.Lifecycle.ResultProcessors
{
    public class HeadResultProcessor : IResultProcessor
    {
        private IResultProcessor _inner;

        public HeadResultProcessor(IResultProcessor innerProcessor)
        {
            _inner = innerProcessor;
        }

        public MatchResult Match(object data, IRequestContext context)
        {
            return _inner.Match(data, context);
        }

        public async Task ProcessHeadersAsync(object data, IRequestContext context)
        {
            await _inner.ProcessHeadersAsync(data, context);
        }

        public async Task ProcessBodyAsync(object data, IRequestContext context)
        {
            await Task.Yield();
        }
    }
}