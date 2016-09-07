using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PeterHenell.SSMS.Plugins.Plugins.Config
{
    public class ConfigurationPlugin : CommandPluginBase
    {
        //private PluginConfigurationManager configManager;
        public PluginConfigurationManager ConfigManager { get; set; }

        //public ConfigurationPlugin(PluginConfigurationManager configManager)
        //    : this()
        //{
        //    this.configManager = configManager;
        //}

        public const string PLUGIN_NAME = "Configuration_Plugin";

        public ConfigurationPlugin()
            : base(PLUGIN_NAME, "About", "Configure Plugins", "global::Ctrl+Alt+A")
        {
        }

        public override void ExecuteCommand(System.Threading.CancellationToken token)
        {
            var form = new ConfigurationForm();
            foreach (var option in ConfigManager.Configurations)
            {
                if (option.Count == 0)
                    continue;

                var configControl = new ConfigControl();

                form.configTabs.Controls.Add(
                    new System.Windows.Forms.TabPage
                {
                    Text = option.OwnerName,
                    Controls =
                    {
                        configControl
                    }
                });

                var dbDict = new DictionaryBindingList<string, string>(option);
                configControl.dataGridView1.DataSource = dbDict;
            }

            form.FormClosing += form_FormClosing;
            form.ShowDialog();
        }

        void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ConfigManager.SaveAll();
        }
    }

    // Binding dictionary to a dataGrid in windows Forms.
    // http://stackoverflow.com/questions/854953/datagridview-bound-to-a-dictionary
    public sealed class Pair<TKey, TValue>
    {
        private readonly TKey key;
        private readonly IDictionary<TKey, TValue> data;

        public Pair(TKey key, IDictionary<TKey, TValue> data)
        {
            this.key = key;
            this.data = data;
        }
        public TKey Key { get { return key; } }
        public TValue Value
        {
            get
            {
                TValue value;
                data.TryGetValue(key, out value);
                return value;
            }
            set
            {
                data[key] = value;
            }
        }
    }
    public class DictionaryBindingList<TKey, TValue>
        : BindingList<Pair<TKey, TValue>>
    {
        private readonly IDictionary<TKey, TValue> data;
        public DictionaryBindingList(IDictionary<TKey, TValue> data)
        {
            this.data = data;
            Reset();
        }
        public void Reset()
        {
            bool oldRaise = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            try
            {
                Clear();
                foreach (TKey key in data.Keys)
                {
                    Add(new Pair<TKey, TValue>(key, data));
                }
            }
            finally
            {
                RaiseListChangedEvents = oldRaise;
                ResetBindings();
            }
        }

    }
}
