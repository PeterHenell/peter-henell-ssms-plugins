using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.Plugins
{
    public class PluginManager<T> where T : class
    {
        readonly List<Type> _loadedPlugins = new List<Type>();
        bool initialized = false;
        // to hold cached instances
        List<T> plugins = new List<T>();

        private T CreateInstance(Type t)
        {
            try
            {
                return Activator.CreateInstance(t) as T;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error trying to create: {0}", t);
                Console.WriteLine(ex.ToString());
                
                throw;
            }
        }

        /// <summary>
        /// Plugins are created at first call and cached for future calls
        /// until LoadAllPlugins are called again.
        /// </summary>
        /// <returns></returns>
        public List<T> GetPluginInstances(Predicate<T> filter)
        {
            if (!initialized)
            {
                foreach (var type in _loadedPlugins)
                {
                    var instance = CreateInstance(type);
                    if (filter(instance))
                    {
                        plugins.Add(instance);    
                    }
                }
                initialized = true;
            }
            
            return plugins;
        }

        public void LoadAllPlugins(string folder) {
            initialized = false;
            foreach (var assemblyName in System.IO.Directory.GetFiles(folder, "*.dll")) 
            {
                var path = System.IO.Path.Combine(folder, assemblyName);

                var loader = new PluginLoader<T>();
                _loadedPlugins.AddRange(loader.LoadPluginTypes(path));    
            }
        }
    }
}
