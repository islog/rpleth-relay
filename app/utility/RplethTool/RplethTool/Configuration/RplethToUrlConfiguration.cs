using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace RplethTool.Configuration
{
    /// <summary>
    /// The rpleth to url configuration class. This is an object for a specific rpleth to url configuration instance to be write in file.
    /// </summary>
    public class RplethToUrlConfiguration : Configuration
    {
        public RplethToUrlStructure struc;

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
        public override bool Write(Interface inter)
        {
            bool res = false;
            System.IO.Directory.CreateDirectory("./" + Name);
            string filename = "./" + Name + "/url.cfg";
            IntPtr ptr_c = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RplethToUrlStructure)));
            FileStream file = null;
            try
            {
                file = new FileStream(filename, FileMode.Create);
                Marshal.StructureToPtr(struc, ptr_c, false);
                for (int i = 0; i < Marshal.SizeOf(typeof(RplethToUrlStructure)); i++)
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
            inter.WriteMessage("\n*** Server configuration ***");
            if (options.Ip == null)
                struc.Ip = inter.GetByteTab("Enter the ip address of the server", 4, '.');
            else
            {
                try
                {
                    struc.Ip = StringHelper.StringToByteTab(options.Ip, 4, '.');
                }
                catch (Exception)
                {
                    struc.Ip = inter.GetByteTab("Enter the ip address of the server", 4, '.');
                }
            }
            if (options.Welcome == null)
                struc.Url = inter.GetMessage("Enter the rest of the server url.\nDon't forget to insert [0] in the location of card number.");
            else
                struc.Url = options.Welcome;
            return res;
        }

        [StructLayout(LayoutKind.Explicit, Size = 260, CharSet = CharSet.Ansi, Pack = 1)]
        public struct RplethToUrlStructure
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
            byte[] ip;

            [FieldOffset(4)]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            string url;

            public byte[] Ip
            {
                get { return ip; }
                set { ip = value; }
            }

            public string Url
            {
                get { return url; }
                set { url = value; }
            }
        }
    }
}
