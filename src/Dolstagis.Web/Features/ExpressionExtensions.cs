using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Dolstagis.Web.Features
{
    public static class ExpressionExtensions
    {
        /// <summary>
        ///  Specifies that this mapping should use a model binder of the
        ///  specified type.
        /// </summary>
        /// <typeparam name="TModelBinder">
        ///  The type of model binder to use. Dependencies will be injected if
        ///  any are defined.
        /// </typeparam>
        /// <param name="expr">
        ///  The configuration expression.
        /// </param>
        /// <returns>
        ///  The configuration expression.
        /// </returns>

        public static IControllerExpression WithModelBinder<TModelBinder>(this IControllerExpression expr)
            where TModelBinder: IModelBinder, new()
        {
            return expr.WithModelBinder(new TModelBinder());
        }


        /// <summary>
        ///  Specifies that the route should bind to a controller of the specified type.
        /// </summary>
        /// <typeparam name="TController">
        ///  The type of controller to use. Dependencies will be injected if any
        ///  are defined.
        /// </typeparam>
        /// <param name="expr">
        ///  The configuration expression.
        /// </param>
        /// <returns>
        ///  The configuration expression.
        /// </returns>

        public static IControllerExpression Controller<TController>(this IRouteDestinationExpression expr)
        {
            return expr.Controller(services => services.GetService<TController>());
        }


        /// <summary>
        ///  Specifies that the route should bind to a controller instantiated by
        ///  the specified factory method.
        /// </summary>
        /// <param name="expr">
        ///  The configuration expression.
        /// </param>
        /// <param name="controllerFunc">
        ///  A function which returns the controller to use.
        /// </param>
        /// <returns>
        ///  The configuration expression.
        /// </returns>

        public static IControllerExpression Controller(this IRouteDestinationExpression expr, Func<object> controllerFunc)
        {
            return expr.Controller(services => controllerFunc());
        }


        /// <summary>
        ///  Specifies that the route should bind to a previously created controller.
        /// </summary>
        /// <param name="expr">
        ///  The configuration expression.
        /// </param>
        /// <param name="reusableController">
        ///  The previously created controller to use to process these requests.
        ///  Note that the controller will be reused across multiple requests,
        ///  therefore it must be thread-safe, and ideally will not have any state.
        ///  Dependencies will be injected into the bound models passed to the
        ///  functions if configured to do so.
        /// </param>
        /// <returns>
        ///  The configuration expression.
        /// </returns>

        public static IControllerExpression Controller(this IRouteDestinationExpression expr, object reusableController)
        {
            return expr.Controller(services => reusableController);
        }


        /* ====== IStaticFilesExpression.FromDirectory ====== */

        /// <summary>
        ///  Specifies that static files should be retrieved from a directory in
        ///  the filespace on the server.
        /// </summary>
        /// <param name="expr">
        ///   The static files configuration expression.
        /// </param>
        /// <param name="rootDirectory">
        ///  The root directory containing the files to fetch.
        /// </param>

        public static void FromDirectory(this IStaticFilesExpression expr, string rootDirectory)
        {
            expr.FromStream((path, services) => {
                var parts = new[] { rootDirectory }.Concat(path.Parts).ToArray();
                var filePath = Path.Combine(parts);
                if (File.Exists(filePath)) {
                    return new FileStream(filePath, FileMode.Open, FileAccess.Read);
                }
                else {
                    return null;
                }
            });
        }


        /* ====== IStaticFilesExpression.FromAssemblyResources ====== */

        /// <summary>
        ///  Specifies that static files should be retrieved from resources in
        ///  the specified assembly, starting at the specified root namespace.
        /// </summary>
        /// <param name="expr">
        ///  The static files configuration expression.
        /// </param>
        /// <param name="assembly">
        ///  The assembly containing this collection of static files.
        /// </param>
        /// <param name="rootNamespace">
        ///  The namespace to be used as the base for this collection of static
        ///  files.
        /// </param>

        public static void FromAssemblyResources
            (this IStaticFilesExpression expr, Assembly assembly, string rootNamespace)
        {
            expr.FromStream((path, services) => {
                var parts = new[] { rootNamespace }.Concat(path.Parts);
                var name = String.Join(".", parts);
                return assembly.GetManifestResourceStream(name);
            });
        }


        /* ====== IStaticFilesExpression.FromAssemblyResourcesRelativeTo ====== */

        /// <summary>
        ///  Specifies that static files should be retrieved from resources in
        ///  the assembly containing the specified type, in the same namespace
        ///  as that type.
        /// </summary>
        /// <typeparam name="TBaseClass">
        ///  The type whose assembly and namespace are to be used as the base
        ///  for this collection of static files.
        /// </typeparam>
        /// <param name="expr">
        ///  The static files configuration expression.
        /// </param>

        public static void FromAssemblyResourcesRelativeTo<TBaseClass>(this IStaticFilesExpression expr)
        {
            expr.FromStream((path, services) => {
                string name = String.Join(".", path.Parts);
                var type = typeof(TBaseClass);
                return type.Assembly.GetManifestResourceStream(type, name);
            });
        }
    }
}
