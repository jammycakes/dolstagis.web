namespace Dolstagis.Web.Lifecycle
{
    public enum Match
    {
        /// <summary>
        ///  The result processor can not process this result.
        /// </summary>
        None,

        /// <summary>
        ///  The result processor will accept anything.
        /// </summary>
        Fallback,

        /// <summary>
        ///  The result processor can process this result, but only because one
        ///  or more matched parameters are fallback parameters.
        /// </summary>
        Inexact,

        /// <summary>
        ///  The result processor is an exact match for this result.
        /// </summary>
        Exact
    }
}
