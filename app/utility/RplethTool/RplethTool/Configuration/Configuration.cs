using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RplethTool.Configuration
{

    /// <summary>
    /// The configuration class. This is an object for a specific configuration instance to be write in file.
    /// </summary>
    public abstract class Configuration
    {
        /// <summary>
        /// The name of the configuration
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Configuration ()
        {
            Name = "Configuration";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the configuration</param>
        public Configuration(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Write the configuration on a file
        /// </summary>
        /// <returns>True if it succesfull or false otherwise</returns>
        public abstract bool Write(Interface inter);

        /// <summary>
        /// Get the configuration from interface
        /// </summary>
        /// <returns>True if it successfull or false otherwise</returns>
        public abstract bool GetConfiguration(Options options, Interface inter);

        /// <summary>
        /// Get the name of the configuration
        /// </summary>
        /// <returns>The name of the configuration</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
