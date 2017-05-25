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
            var manager = new CommandPluginManager();
            manager.LoadAllPlugins(System.Environment.CurrentDirectory);
            var plugins = manager.GetPluginInstances();

            Assert.That(plugins.Count, Is.GreaterThanOrEqualTo(5).And.LessThanOrEqualTo(50));
        }

        [Test]
        public void AllPluginsMustHaveMenuGroup()
        {
            var manager = new CommandPluginManager();
            manager.LoadAllPlugins(System.Environment.CurrentDirectory);
            var plugins = manager.GetPluginInstances();

            Console.WriteLine(plugins.Count);
            foreach (var pl in plugins)
            {
                Console.WriteLine("Validating: " + pl.Name);
                Assert.That(pl.MenuGroup, Is.Not.Null.Or.Empty);
                Assert.That(pl.Name, Is.Not.Null.And.Not.Empty);
                Assert.That(pl.DefaultBindings, Is.Not.Null);
                Assert.That(pl.Enabled, Is.True);
            }
        }

        [Test]
        [Ignore("We should probably not cache since we want to be able to reload.")]
        public void PluginsShouldBeCached()
        {
            var manager = new CommandPluginManager();
            manager.LoadAllPlugins(System.Environment.CurrentDirectory);
            var plugins = manager.GetPluginInstances();
            var pluginsAgain = manager.GetPluginInstances();

            CollectionAssert.AreEquivalent(plugins, pluginsAgain);
        }

        [Test]
        public void PluginsWillNotBeLoadedIfTheyDoNotHaveIconSet()
        {
            var manager = new CommandPluginManager();
            manager.LoadAllPlugins(System.Environment.CurrentDirectory);
            var plugins = manager.GetPluginInstances();

            var bad = plugins.FirstOrDefault(x => x.Name == "Bad Mock");
            Assert.That(bad, Is.Null);
        }
    }

    public class BadPluginMock : ICommandPlugin
    {

        public string MenuGroup
        {
            get { throw new NotImplementedException(); }
        }

        public void Init(ISsmsFunctionalityProvider4 provider)
        {
            throw new NotImplementedException();
        }

        public string Caption
        {
            get { throw new NotImplementedException(); }
        }

        public string[] DefaultBindings
        {
            get { throw new NotImplementedException(); }
        }

        public bool Enabled
        {
            get { throw new NotImplementedException(); }
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public ICommandImage Icon
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { return "Bad Mock"; }
        }

        public string Tooltip
        {
            get { throw new NotImplementedException(); }
        }

        public bool Visible
        {
            get { throw new NotImplementedException(); }
        }
    }
}
