using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RplethTool.Configuration
{
    /// <summary>
    /// The rpleht reader configuration class. This is an object for a specific configuration instance to be write in file.
    /// </summary>
    public class RplethReaderConfiguration : Configuration
    {
        public RplethReaderStructure struc;

        /// <summary>
        /// Constructor
        /// </summary>
        public RplethReaderConfiguration() 
            : base()
        {
            Name = "RplethReaderConfiguration";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the configuration</param>
        public RplethReaderConfiguration(string  name)
            : base(name)
        {

        }

        /// <summary>
        /// Write the configuration on a file
        /// </summary>
        /// <returns>True if it succesfull or false otherwise</returns>
        public override bool Work(Interface inter, Options option)
        {
            bool res = false;
            System.IO.Directory.CreateDirectory("./" + Name);
            string filename = "./" + Name + "/reader.cfg";
            IntPtr ptr_c = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RplethReaderStructure)));
            FileStream file = null;
            try
            {
                file = new FileStream(filename, FileMode.Create);
                Marshal.StructureToPtr(struc, ptr_c, false);
                for (int i = 0; i < Marshal.SizeOf(typeof(RplethReaderStructure)); i++)
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
                inter.WriteMessage("\n*** Ethernet configuration ***\n");
                if (options.Mac == null)
                    struc.Mac = inter.GetByteTab("Enter the mac address of Rpleth", 6, ':', 16, false);
                else
                {
                    try
                    {
                        struc.Mac = StringHelper.StringToByteTab(options.Mac, 6, ':', 16);
                    }
                    catch (Exception)
                    {
                        struc.Mac = inter.GetByteTab("Enter the mac address of Rpleth", 6, ':', 16, false);
                    }
                }
                if (options.Dhcp == -1)
                    struc.Dhcp = inter.GetByte("Disable the dhcp ?\nEnter 0 to disable or 1 to enable.", false);
                else
                    struc.Dhcp = Convert.ToByte(options.Dhcp);
                if (struc.Dhcp == 0)
                {
                    if (options.Ip == null)
                        struc.Ip = inter.GetByteTab("Enter the ip address of Rpleth", 4, '.', false);
                    else
                    {
                        try
                        {
                            struc.Ip = StringHelper.StringToByteTab(options.Ip, 4, '.');
                        }
                        catch (Exception)
                        {
                            struc.Ip = inter.GetByteTab("Enter the ip address of Rpleth", 4, '.', false);
                        }
                    }
            
                    if (options.Subnet == null)
                        struc.Subnet = inter.GetByteTab("Enter the subnet address for Rpleth", 4, '.', false);
                    else
                    {
                        try
                        {
                            struc.Subnet = StringHelper.StringToByteTab(options.Subnet, 4, '.');
                        }
                        catch (Exception)
                        {
                            struc.Subnet = inter.GetByteTab("Enter the subnet address for Rpleth", 4, '.', false);
                        }
                    }
                    if (options.Gateway == null)
                        struc.Gateway = inter.GetByteTab("Enter the gateway address for Rpleth", 4, '.', false);
                    else
                    {
                        try
                        {
                            struc.Gateway = StringHelper.StringToByteTab(options.Gateway, 4, '.');
                        }
                        catch (Exception)
                        {
                            struc.Gateway = inter.GetByteTab("Enter the gateway address for Rpleth", 4, '.', false);
                        }
                    }
                }
                if (options.Port == -1)
                    struc.Port = Convert.ToUInt16(inter.GetUint("Enter the port number that listen Rpleth", false));
                else
                    struc.Port = Convert.ToUInt16(options.Port);
                inter.WriteMessage("\n*** Rpleth configuration ***\n");
                if (options.Welcome == null)
                    struc.Message = inter.GetMessage("Enter the welcome message of Rpleth", false);
                else
                    struc.Message = options.Welcome;
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
        [StructLayout(LayoutKind.Explicit, Size = 57, CharSet = CharSet.Ansi, Pack = 1)]
        public struct RplethReaderStructure
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
            byte[] ip;

            [FieldOffset(4)]
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
            byte[] dns;

            [FieldOffset(8)]
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
            byte[] subnet;

            [FieldOffset(12)]
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
            byte[] gateway;

            [FieldOffset(16)]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            string message;

            [FieldOffset(48)]
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 6)]
            byte[] mac;

            [FieldOffset(54)]
            [MarshalAs(UnmanagedType.U2)]
            ushort port;

            [FieldOffset(56)]
            [MarshalAs(UnmanagedType.U1)]
            byte dhcp;

            public byte[] Ip
            {
                get { return ip; }
                set { ip = value; }
            }
            public byte[] Dns
            {
                get { return dns; }
                set { dns = value; }
            }
            public byte[] Subnet
            {
                get { return subnet; }
                set { subnet = value; }
            }
            public byte[] Gateway
            {
                get { return gateway; }
                set { gateway = value; }
            }
            public byte[] Mac
            {
                get { return mac; }
                set { mac = value; }
            }
            public ushort Port
            {
                get { return port; }
                set { port = value; }
            }
            public byte Dhcp
            {
                get { return dhcp; }
                set { dhcp = value; }
            }
            public string Message
            {
                get { return message; }
                set { message = value; }
            }
        }
    }
}
