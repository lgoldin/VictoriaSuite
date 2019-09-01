using System;
using System.IO;
using Newtonsoft.Json;

namespace Victoria.Shared
{
    public static class ObjectCloneHelper
    {
        /// <summary>
        /// Perform a deep Copy of the object, using Json as a serialisation method.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(System.AppDomain));
        public static T CloneJson<T>(this T source)
        {
            //logger.Info("Inicio Clone Json");
            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                //logger.Info("Fin Clone Json Default");
                return default(T);
            }

            //logger.Info("Fin Clone Json");
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }

    }
}
