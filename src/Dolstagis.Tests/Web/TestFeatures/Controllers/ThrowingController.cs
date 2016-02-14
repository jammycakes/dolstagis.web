using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Tests.Web.TestFeatures.Controllers
{
    public class ThrowingController<TException> where TException: Exception, new()
    {
        private bool _throwAsync;
        private bool _afterAwait;

        public ThrowingController(bool throwAsync, bool afterAwait)
        {
            _throwAsync = throwAsync;
            _afterAwait = afterAwait;
        }

        private async Task<object> GetAsync()
        {
            if (!_afterAwait) throw new TException();
            await Task.Delay(1);
            if (_afterAwait) throw new TException();
            return null;
        }

        public object Get()
        {
            if (_throwAsync) {
                return GetAsync();
            }
            else {
                throw new TException();
            }
        }
    }
}
