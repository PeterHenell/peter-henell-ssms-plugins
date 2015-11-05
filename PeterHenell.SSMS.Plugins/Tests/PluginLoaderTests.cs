using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using PeterHenell.SSMS.Plugins.Plugins;
using System.IO;
using RedGate.SIPFrameworkShared;

namespace PeterHenell.SSMS.Plugins.PluggableCommands.Tests
{
    
    [TestFixture]
    public class PluginLoaderTests
    {
        [Test]
        public void ShouldLoadPlugin()
        {
            var loader = new PluginLoader<ICommandPlugin>();
            var path = Path.Combine(System.Environment.CurrentDirectory, "PeterHenell.SSMS.Plugins.dll");
            var plugins = loader.LoadPluginTypes(path);

            foreach (var pl in plugins)
            {
                Console.WriteLine(pl.Name);
                Console.WriteLine();
            }

            Assert.That(plugins.Count, Is.GreaterThanOrEqualTo(5).And.LessThanOrEqualTo(50));
        }

        [Test]
        public void ShouldInstatiateAllPlugins()
        {
            PluginManager<ICommandPlugin> manager = new PluginManager<ICommandPlugin>();
            manager.LoadAllPlugins(System.Environment.CurrentDirectory);
            var plugins = manager.GetPluginInstances(i => i.Enabled);

            Assert.That(plugins.Count, Is.GreaterThanOrEqualTo(5).And.LessThanOrEqualTo(50));
        }

        [Test]
        public void AllPluginsMustHaveMenuGroup()
        {
            PluginManager<ICommandPlugin> manager = new PluginManager<ICommandPlugin>();
            manager.LoadAllPlugins(System.Environment.CurrentDirectory);
            var plugins = manager.GetPluginInstances(i => i.Enabled);

            Console.WriteLine(plugins.Count);
            foreach (var pl in plugins)
            {
                Console.WriteLine("Validating: " + pl.Name);
                Assert.That(pl.MenuGroup,Is.Not.Null.Or.Empty);
                Assert.That(pl.Name, Is.Not.Null.And.Not.Empty);
                Assert.That(pl.DefaultBindings, Is.Not.Null);
                Assert.That(pl.Enabled, Is.True);
            }
        }

        [Test]
        public void PluginsShouldBeCached()
        {
            PluginManager<ICommandPlugin> manager = new PluginManager<ICommandPlugin>();
            manager.LoadAllPlugins(System.Environment.CurrentDirectory);
            var plugins = manager.GetPluginInstances(i => i.Enabled);
            var pluginsAgain = manager.GetPluginInstances(i => i.Enabled);

            CollectionAssert.AreEqual(plugins, pluginsAgain);
        }
    }
}
