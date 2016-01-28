using System;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Features.Impl
{
    public class FeatureSwitch : ISwitchExpression, IFeatureSwitch
    {
        private Predicate<IRequest> _predicate = null;

        public bool IsEnabledForRequest(IRequest request)
        {
            return _predicate == null || _predicate(request);
        }

        public void When(Predicate<IRequest> predicate)
        {
            _predicate = predicate;
        }

        public void When(Func<bool> predicate)
        {
            _predicate = request => predicate();
        }
    }
}
