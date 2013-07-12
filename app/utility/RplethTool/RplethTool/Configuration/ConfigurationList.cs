using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RplethTool.Configuration
{
    /// <summary>
    /// This class provide list of all the configuration.
    /// </summary>
    public class ConfigurationList
    {
        /// <summary>
        /// The list of all the configuration.
        /// </summary>
        public List<Configuration> ConfigurationsList { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigurationList ()
        {
            ConfigurationsList = new List<Configuration>();
            FillList();
        }

        /// <summary>
        /// Fill the ConfigurationList
        /// </summary>
        public void FillList()
        {
            Assembly rplethToolAssembly = typeof(Configuration).Assembly;
            foreach (Type type in rplethToolAssembly.GetTypes())
            {
                try
                {
                    string[] names = type.FullName.Split('.');
                    if (names.Length == 3)
                    {
                        if (names[1] == "Configuration" && names[2] != "Configuration" && names[2] != "ConfigurationList" && !names[2].Contains('+'))
                            ConfigurationsList.Add(Activator.CreateInstance(type) as Configuration);
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
