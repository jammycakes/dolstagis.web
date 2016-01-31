namespace Dolstagis.Web
{
    public enum Scope
    {
        /// <summary>
        ///  One instance of the service is created at the level at which it is
        ///  declared. It will be disposed when the application terminates, or
        ///  when the feature context in which it is created is disposed.
        /// </summary>

        Application,

        /// <summary>
        ///  One instance of the service is created for each HTTP request, and
        ///  will be disposed when the request has finished processing.
        /// </summary>

        Request,

        /// <summary>
        ///  One instance of the service is created for each dependency graph
        ///  into which it is injected. It will not be automatically disposed.
        /// </summary>

        Transient
    }
}
