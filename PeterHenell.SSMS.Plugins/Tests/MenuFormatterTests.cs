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
            var plugins = new List<ICommandPlugin>();
            plugins.Add(new MockCommandPlugin("", "HOME"));
            plugins.Add(new MockCommandPlugin("", "HOME"));
            plugins.Add(new MockCommandPlugin("", "HOME"));

            plugins.Add(new MockCommandPlugin("", "UTILS"));
            plugins.Add(new MockCommandPlugin("", "UTILS"));

            var groups = formatter.GetMenuGroups(plugins);
            Assert.That(groups.Keys.Count, Is.EqualTo(2));

            var home = groups["HOME"];
            Assert.That(home.Count, Is.EqualTo(3));

            var utils = groups["UTILS"]; 
            Assert.That(utils.Count(), Is.EqualTo(2));
        }
    }
}
