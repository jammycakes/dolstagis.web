using System;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Features.Impl
{
    public class FeatureSwitch : ISwitchExpression, IFeatureSwitch
    {
        private Predicate<IRequest> _predicate = null;

        public event EventHandler Defining;

        public FeatureSwitch()
        {
        }

        public bool IsEnabledForRequest(IRequest request)
        {
            return _predicate == null || _predicate(request);
        }

        public void When(Predicate<IRequest> predicate)
        {
            if (Defining != null) Defining(this, EventArgs.Empty);
            _predicate = predicate;
        }

        public void When(Func<bool> predicate)
        {
            if (Defining != null) Defining(this, EventArgs.Empty);
            _predicate = request => predicate();
        }

        public void AssertNotDefined(string errorMessage)
        {
            if (_predicate != null)
                throw new InvalidOperationException(errorMessage);
        }
    }
}
