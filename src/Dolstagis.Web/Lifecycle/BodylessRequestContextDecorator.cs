using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Auth;
using Dolstagis.Web.Http;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Sessions;

namespace Dolstagis.Web.Lifecycle
{
    public class BodylessRequestContextDecorator : IRequestContext
    {
        private IRequestContext _inner;
        private IResponse _response;

        public BodylessRequestContextDecorator(IRequestContext innerContext)
        {
            _inner = innerContext;
            _response = new BodylessResponseDecorator(innerContext.Response);
        }

        public IServiceLocator Container {  get { return _inner.Container; } }

        public IRequest Request { get { return _inner.Request; } }

        public IResponse Response { get { return _response; } }

        public ISession Session { get { return _inner.Session; } }

        public IUser User { get { return _inner.User;  } }

        public Task<ISession> GetSessionAsync()
        {
            return _inner.GetSessionAsync();
        }

        public Task<IUser> GetUserAsync()
        {
            return _inner.GetUserAsync();
        }

        private class BodylessResponseDecorator : IResponse
        {
            private IResponse _inner;

            public BodylessResponseDecorator(IResponse inner)
            {
                _inner = inner;
            }

            public Stream Body { get { return Stream.Null; } }

            public ResponseHeaders Headers { get { return _inner.Headers; } }

            public string Protocol
            {
                get { return _inner.Protocol; }
                set { _inner.Protocol = value; }
            }

            public Status Status
            {
                get { return _inner.Status; }
                set { _inner.Status = value; }
            }

            public void AddHeader(string name, string value)
            {
                _inner.AddHeader(name, value);
            }
        }
    }
}
