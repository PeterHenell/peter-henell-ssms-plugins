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

        //"{0}\\Plugins\\{1}", System.Environment.CurrentDirectory
        public void LoadPlugin(string pluginFolder, string assemblyName)
        {
            try
            {
                string assemblyFileName = String.Format(pluginFolder, assemblyName);
                Assembly asm = Assembly.LoadFrom(assemblyFileName);
                Type[] types = asm.GetTypes();
                foreach (Type thisType in types)
                {
                    Type[] interfaces = thisType.GetInterfaces();
                    foreach (Type thisInterface in interfaces)
                    {
                        if (thisInterface.GetInterfaces().Any(x => x.IsInstanceOfType(typeof(T))))
                        {
                            //-- Load the object
                            _loadedPlugins.Add(thisType);
                            //object obj = asm.CreateInstance(String.Format("{0}.{1}", thisType.Namespace, thisType.Name));
                            //return obj as T;
                        }
                    }

                }
            }
            catch
            {
                // This should be logged
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
