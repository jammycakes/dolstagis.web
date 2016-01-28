using Dolstagis.Web.Http;

namespace Dolstagis.Web.Features
{
    /// <summary>
    ///  Indicates whether a feature is enabled or disabled for a particular
    ///  request.
    /// </summary>

    public interface IFeatureSwitch
    {
        /// <summary>
        ///  Queries the given request to determine whether or not this feature
        ///  is enabled for this request.
        /// </summary>
        /// <param name="request">
        ///  The current request.
        /// </param>
        /// <returns>
        ///  true if the feature is enabled for this request, otherwise false.
        /// </returns>

        bool IsEnabledForRequest(IRequest request);
    }
}
