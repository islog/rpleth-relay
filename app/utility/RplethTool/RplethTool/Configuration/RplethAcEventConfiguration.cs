using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace RplethTool.Configuration
{
    /// <summary>
    /// The RplethAcEvent configuration class. This is an object for a specific RplethAcEvent configuration instance to be write in file.
    /// </summary>
    class RplethAcEventConfiguration : Configuration
    {
        /// <summary>
        /// Represente the structure to be write in file.
        /// </summary>
        public RplethAcEventStructure struc;

        /// <summary>
        /// Constructor.
        /// </summary>
        public RplethAcEventConfiguration()
            : base ()
        {
            Name = "RplethAcEventConfiguration";
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the configuration.</param>
        public RplethAcEventConfiguration(string name)
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
            if (options.Begin == -1) 
                struc.Begin = Convert.ToUInt16(options.Begin);
            else
                struc.Begin = Convert.ToUInt16(inter.GetUint("Enter the begin number"));
            return res;
        }

        /// <summary>
        /// The structure use to write on file.
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = 39, CharSet = CharSet.Ansi, Pack = 1)]
        public struct RplethAcEventStructure
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.I2)]
            private ushort beginNumber;
            [FieldOffset(2)]
            [MarshalAs(UnmanagedType.I2)]
            private ushort cancelNumber;
            [FieldOffset(4)]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            private string user;
            [FieldOffset(16)]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            private string pass;
            [FieldOffset(28)]
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
            private byte[] ip;
            [FieldOffset(32)]
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 6)]
            private byte[] mac;
            [FieldOffset(38)]
            [MarshalAs(UnmanagedType.U1)]
            private byte tram_size;

            public ushort Begin
            {
                get { return beginNumber; }
                set { beginNumber = value; }
            }

            public ushort Cancel
            {
                get { return cancelNumber; }
                set { cancelNumber = value; }
            }

            public string User
            {
                get { return user; }
                set { user = value; }
            }

            public string Pass
            {
                get { return pass; }
                set { pass = value; }
            }

            private byte[] Ip
            {
                get { return ip; }
                set { ip = value; }
            }

            private byte[] Mac
            {
                get { return mac; }
                set { mac = value; }
            }

            private byte Tram_size
            {
                get { return tram_size; }
                set { tram_size = value; }
            }
        }
    }
}
