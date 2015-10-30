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
        readonly List<Type> _loadedPlugins;

        public PluginLoader()
        {
            _loadedPlugins = new List<Type>();
        }

        public void LoadPlugin(string pluginFolder, string assemblyName)
        {
            try
            {
                string assemblyFileName = System.IO.Path.Combine(pluginFolder, assemblyName);
                Assembly asm = Assembly.LoadFrom(assemblyFileName);
                Type[] typesInTheAssembly = asm.GetTypes();
                foreach (Type currentType in typesInTheAssembly)
                {
                    Type[] interfacesOfTheType = currentType.GetInterfaces();
                    foreach (Type thisInterface in interfacesOfTheType)
                    {
                        Console.WriteLine(thisInterface.AssemblyQualifiedName);
                        if (thisInterface.IsInstanceOfType(typeof(T)))
                        {
                            _loadedPlugins.Add(currentType);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                // This should be logged
                Console.WriteLine(e.ToString());
            }
        }

        private T CreateInstance(Type t)
        {
            return Activator.CreateInstance(t) as T;
        }

        public List<T> CreateInstances() 
        {
            var plugins = new List<T>();
            foreach (var type in _loadedPlugins)
            {
                plugins.Add(CreateInstance(type));
            }
            return plugins;
        }
    }
}
