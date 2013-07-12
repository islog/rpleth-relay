using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace RplethTool
{
    /// <summary>
    /// Class to parse the command line args variable.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Selected menu.
        /// </summary>
        [Option('n', "menu", DefaultValue = -1, HelpText = "Selected menu.")]
        public int Menu { get; set; }

        /// <summary>
        /// The begin number of wiegand com.
        /// </summary>
        [Option('b', "begin", DefaultValue = -1, HelpText = "The begin number of wiegand com.")]
        public int Begin { get; set; }

        /// <summary>
        /// The cancel number of wiegand com.
        /// </summary>
        [Option('c', "cancel", DefaultValue = -1, HelpText = "The cancel number of wiegand com.")]
        public int Cancel { get; set; }
        
        /// <summary>
        /// The offset in number of wiegand com.
        /// </summary>
        [Option('o', "offset", DefaultValue = -1, HelpText = "The offset in number of wiegand com.")]
        public int Offset { get; set; }

        /// <summary>
        /// The lenght of number in wiegand com.
        /// </summary>
        [Option('l', "lenght", DefaultValue = -1, HelpText = "The lenght of number in wiegand com.")]
        public int Lenght { get; set; }

        /// <summary>
        /// The size of trame in wiegand com.
        /// </summary>
        [Option('t', "tramsize", DefaultValue = -1, HelpText = "The size of trame in wiegand com.")]
        public int TramSize { get; set; }

        /// <summary>
        /// The ip adress of Arduino.
        /// </summary>
        [Option('i', "ip", DefaultValue = null, HelpText = "The ip adress of Arduino.")]
        public string Ip { get; set; }

        /// <summary>
        /// The dns adress of Arduino.
        /// </summary>
        [Option('d', "dns", DefaultValue = null, HelpText = "The dns adress of Arduino.")]
        public string Dns { get; set; }

        /// <summary>
        /// The subnet mask of Arduino.
        /// </summary>
        [Option('s', "subnet", DefaultValue = null, HelpText = "The subnet mask of Arduino.")]
        public string Subnet { get; set; }

        /// <summary>
        /// The gateway adress of Arduino.
        /// </summary>
        [Option('g', "gateway", DefaultValue = null, HelpText = "The gateway adress of Arduino.")]
        public string Gateway { get; set; }

        /// <summary>
        /// The welcome message of Arduino.
        /// </summary>
        [Option('w', "welcome", DefaultValue = null, HelpText = "The welcome message of Arduino.")]
        public string Welcome { get; set; }

        /// <summary>
        /// The mac adress of Arduino.
        /// </summary>
        [Option('m', "mac", DefaultValue = null, HelpText = "The mac adress of Arduino.")]
        public string Mac { get; set; }

        /// <summary>
        /// The port of Arduino.
        /// </summary>
        [Option('p', "port", DefaultValue = -1, HelpText = "The port of Arduino.")]
        public int Port { get; set; }

        /// <summary>
        /// Stat of dhcp. (0 to disable or anything else to enable)
        /// </summary>
        [Option('x', "dhcp", DefaultValue = -1, HelpText = "Stat of dhcp. (0 to disable or anything else to enable)")]
        public int Dhcp { get; set; }

        /// <summary>
        /// User of arduino in ftp server.
        /// </summary>
        [Option('u', "user", DefaultValue = null, HelpText = "User of arduino in ftp server.")]
        public string User { get; set; }

        /// <summary>
        /// Password of arduino in ftp server.
        /// </summary>
        [Option('f', "pass", DefaultValue = null, HelpText = "Password of arduino in ftp server.")]
        public string Pass { get; set; }

        /// <summary>
        /// Display the help message.
        /// </summary>
        /// <returns>The help message</returns>
        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
            current => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        /// <summary>
        /// Parse the args in options.
        /// </summary>
        /// <param name="args">Args get in command line</param>
        public void Parse (string[] args)
        {
            if (!CommandLine.Parser.Default.ParseArguments(args, this))
            {
                throw (new Exception("Error : Invalide arguments"));
            }
        }
    }
}
