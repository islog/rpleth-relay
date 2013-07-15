using System;
using System.Collections.Generic;
using System.IO;
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
        public override bool Write(Interface inter)
        {
            bool res = false;
            System.IO.Directory.CreateDirectory("./"+Name);
            string filename = "./"+Name+"/acevent.cfg";
            IntPtr ptr_c = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RplethAcEventStructure)));
            FileStream file = null;
            try
            {
                file = new FileStream(filename, FileMode.Create);
                Marshal.StructureToPtr(struc, ptr_c, false);
                for (int i = 0; i < Marshal.SizeOf(typeof(RplethAcEventStructure)); i++)
                {
                    file.WriteByte(Marshal.ReadByte(ptr_c, i));
                }
                inter.WriteMessage("\n\t\t\t***********************");
                inter.WriteMessage("\tThe configuration file has been successfully writing.");
                inter.WriteMessage("\t\t\t***********************\n");
                res = true;
            }
            catch (Exception ex)
            {
                inter.WriteError(ex.Message);
                inter.WriteMessage("\n\t\t\t***********************");
                inter.WriteMessage("\tThe configuration file hasn't been successfully writing.");
                inter.WriteMessage("\t\t\t***********************\n");
            }
            finally
            {
                if (file != null)
                    file.Flush();
                file.Close();
                Marshal.FreeHGlobal(ptr_c);
            }
            return res;
        }

        /// <summary>
        /// Get the configuration from interface
        /// </summary>
        /// <returns>True if it successfull or false otherwise</returns>
        public override bool GetConfiguration(Options options, Interface inter)
        {
            bool res = true;
            try
            {
                inter.WriteMessage("\n*** Wiegand configuration ***\n");
                if (options.Begin == -1)
                    struc.Begin = Convert.ToUInt16(inter.GetUint("Enter the number of begin card"));
                else
                    struc.Begin = Convert.ToUInt16(options.Begin);
                if (options.Cancel == -1)
                    struc.Cancel = Convert.ToUInt16(inter.GetUint("Enter the number of cancel card"));
                else
                    struc.Cancel = Convert.ToUInt16(options.Begin);
                if (options.TramSize == -1)
                    struc.TrameSize = inter.GetByte("Enter the size of wiegand frame read by Rpleth");
                else
                    struc.TrameSize = Convert.ToByte(options.TramSize);
                inter.WriteMessage("\n*** Ethernet configuration ***\n");
                if (options.Mac == null)
                    struc.Mac = inter.GetByteTab("Enter the mac address of Rpleth", 6, ':', 16);
                else
                {
                    try
                    {
                        struc.Mac = StringHelper.StringToByteTab(options.Mac, 6, ':', 16);
                    }
                    catch (Exception)
                    {
                        struc.Mac = inter.GetByteTab("Enter the mac address of Rpleth", 6, ':', 16);
                    }
                }
                inter.WriteMessage("\n*** Ftp server configuration ***\n");
                if (options.Ip == null)
                    struc.Ip = inter.GetByteTab("Enter the ip address of ftp server", 4, '.');
                else
                {
                    try
                    {
                        struc.Ip = StringHelper.StringToByteTab(options.Ip, 4, '.');
                    }
                    catch (Exception)
                    {
                        struc.Ip = inter.GetByteTab("Enter the ip address of ftp server", 4, '.');
                    }
                }
                if (options.User == null)
                    struc.User = inter.GetMessage("Enter the user of ftp server");
                else
                    struc.User = options.User;
                if (options.Pass == null)
                    struc.Pass = inter.GetMessage("Enter the password of ftp server");
                else
                    struc.Pass = options.Pass;
            }
            catch (Exception)
            {
                res = false;
            }

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
            ushort beginNumber;

            [FieldOffset(2)]
            [MarshalAs(UnmanagedType.I2)]
            ushort cancelNumber;

            [FieldOffset(4)]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            string user;

            [FieldOffset(16)]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            string pass;

            [FieldOffset(28)]
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
            byte[] ip;

            [FieldOffset(32)]
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 6)]
            byte[] mac;

            [FieldOffset(38)]
            [MarshalAs(UnmanagedType.U1)]
            byte tram_size;

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

            public byte[] Ip
            {
                get { return ip; }
                set { ip = value; }
            }

            public byte[] Mac
            {
                get { return mac; }
                set { mac = value; }
            }

            public byte TrameSize
            {
                get { return tram_size; }
                set { tram_size = value; }
            }
        }
    }
}
