using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RplethTool.Configuration
{
    /// <summary>
    /// The rpleth to url configuration class. This is an object for a specific rpleth to url configuration instance to be write in file.
    /// </summary>
    public class RplethToUrlConfiguration : Configuration
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RplethToUrlConfiguration()
            : base ()
        {
            Name = "RplethToUrlConfiguration";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the configuration</param>
        public RplethToUrlConfiguration(string name)
            : base(name)
        {

        }

        /// <summary>
        /// Write the configuration on a file
        /// </summary>
        /// <returns>True if it succesfull or false otherwise</returns>
        public override bool Write()
        {
            bool res = false;

            return res;
        }

        /// <summary>
        /// Get the configuration from interface
        /// </summary>
        /// <returns>True if it successfull or false otherwise</returns>
        public override bool GetConfiguration(Options options, Interface inter)
        {
            bool res = false;

            return res;
        }
    }
}
