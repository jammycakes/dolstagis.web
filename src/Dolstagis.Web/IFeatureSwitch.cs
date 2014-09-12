using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web
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
        /// <remarks>
        ///  If <see cref="DependentOnRequest"/> returns false, this method
        ///  MUST ignore the value of request and all its properties.
        /// </remarks>

        Task<bool> IsEnabledForRequest(IRequest request);

        /// <summary>
        ///  Gets the feature which is controlled by this switch.
        /// </summary>

        Feature Feature { get; }

        /// <summary>
        ///  Gets a value indicating whether this feature switch is dependent
        ///  on the request.
        /// </summary>
        /// <remarks>
        ///  This property MUST be set to true if <see cref="IsEnabledForRequest"/>
        ///  returns a value which is dependent on the request. It SHOULD be set
        ///  to false otherwise.
        ///  
        ///  Each application is limited to a maximum of sixty-four request
        ///  dependent features. The number of features SHOULD be kept
        ///  mucg lower than that anyway otherwise the application may run out
        ///  of memory.
        /// </remarks>

        bool DependentOnRequest { get; }
    }
}
