using NUnit.Framework;
using PeterHenell.SSMS.Plugins.Plugins;
using PeterHenell.SSMS.Plugins.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.Tests
{
    [TestFixture]
    class MenuFormatterTests
    {

        MenuFormatter formatter = new MenuFormatter();

        [Test]
        public void ShouldGroupMenuItemsByName()
        {
            var plugins = new List<CommandPluginBase>();
            plugins.Add(new MockCommandPlugin("A", "HOME"));
            plugins.Add(new MockCommandPlugin("B", "HOME"));
            plugins.Add(new MockCommandPlugin("C", "HOME"));

            plugins.Add(new MockCommandPlugin("D", "UTILS"));
            plugins.Add(new MockCommandPlugin("E", "UTILS"));

            var groups = formatter.GetMenuGroups(plugins.Select(p => new CommandPluginWrapper(p)));
            Assert.That(groups.Keys.Count, Is.EqualTo(2));

            var home = groups["HOME"];
            Assert.That(home.Count, Is.EqualTo(3));

            var utils = groups["UTILS"]; 
            Assert.That(utils.Count(), Is.EqualTo(2));
        }

        [Test]
        public void ShouldCreateGroupsFromInstances()
        {
            var manager = new CommandPluginManager();
            manager.LoadAllPlugins(System.Environment.CurrentDirectory);
            var plugins = manager.GetPluginInstances();

            var groups = formatter.GetMenuGroups(plugins);
            
            Console.WriteLine(plugins.Count);
        }
    }
}
