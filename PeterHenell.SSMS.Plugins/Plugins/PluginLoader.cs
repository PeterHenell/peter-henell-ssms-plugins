using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.Plugins
{
    public class PluginLoader<T> where T : class
    {
        public PluginLoader()
        {

        }

        public List<Type> LoadPluginTypes(string assemblyFileName)
        {
            var loadedPlugins = new List<Type>();
            try
            {
                Assembly asm = Assembly.LoadFrom(assemblyFileName);
                Type[] typesInTheAssembly = asm.GetTypes();
                foreach (Type currentType in typesInTheAssembly)
                {
                    Type[] interfacesOfTheType = currentType.GetInterfaces();
                    if (typeof(T).IsAssignableFrom(currentType) && !currentType.IsAbstract)
                    {
                        Console.WriteLine(currentType.AssemblyQualifiedName);
                        loadedPlugins.Add(currentType);
                    }
                }
            }
            catch (Exception e)
            {
                // This should be logged
                Console.WriteLine(e.ToString());
            }
            return loadedPlugins;
        }


    }
}
