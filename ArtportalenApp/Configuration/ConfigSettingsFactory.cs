using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace ArtportalenApp.Configuration
{
    public class ConfigSettingsFactory
    {
        public static T Load<T>() where T : new()
        {
            var properties = typeof (T).GetRuntimeProperties().ToDictionary(p => p.Name);

            var configType = typeof (T);
            var assemblyType = typeof (App);
            var configFileName = LowerCaseFirst(configType.Name);
            var resource = string.Format("{0}.{1}.json", assemblyType.Namespace, configFileName);

            try
            {
                using (var stream = assemblyType.GetTypeInfo().Assembly.GetManifestResourceStream(resource))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Embedded resource '{0}' could not be interpreted: {1}", resource, e.Message));
            }
        }

        private static string LowerCaseFirst(string name)
        {
            return name.Substring(0, 1).ToLower() + name.Substring(1);
        }
    }
}