using System;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Features
{
    public interface ISwitchExpression
    {
        /// <summary>
        ///  Indicates that the feature will be active when the given condition
        ///  is satisfied.
        /// </summary>
        /// <param name="condition"></param>

        void When(Func<bool> condition);

        /// <summary>
        ///  Configures the feature to be active when the request satisfies the
        ///  given condition.
        /// </summary>
        /// <param name="condition"></param>

        void When(Predicate<IRequest> condition);
    }
}
