using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dolstagis.Web.Util
{
    public static class AssemblyExtensions
    {
        /// <summary>
        ///  Gets all types in an assembly, catching ReflectionTypeLoadException
        /// </summary>
        /// <remarks>
        ///  See http://haacked.com/archive/2012/07/23/get-all-types-in-an-assembly.aspx/
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>

        public static IEnumerable<Type> SafeGetTypes<T>(this Assembly assembly)
        {
            IEnumerable<Type> types;

            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types.Where(t => t != null);
            }

            return types.Where(t => typeof(T).IsAssignableFrom(t));
        }


        /// <summary>
        ///  Gets instances of all types in an assembly, without throwing
        ///  ReflectionTypeLoadException.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>

        public static IEnumerable<T> SafeGetInstances<T>(this Assembly assembly)
        {
            return
                from type in assembly.SafeGetTypes<T>()
                where !type.IsAbstract
                let constructor = type.GetConstructor(Type.EmptyTypes)
                where constructor != null
                select (T)constructor.Invoke(null);
        }
    }
}
