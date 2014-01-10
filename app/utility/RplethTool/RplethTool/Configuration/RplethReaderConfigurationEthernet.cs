using LibLogicalAccess;
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
    public class RplethReaderConfigurationEthernet : Configuration
    {
        public RplethReaderStructure struc;
        public string TargetIp;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public RplethReaderConfigurationEthernet() 
            : base()
        {
            Name = "Configure a Rpleth by Ethernet";

            struc.Dhcp = 3;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the configuration</param>
        public RplethReaderConfigurationEthernet(string  name)
            : base(name)
        {

        }

        /// <summary>
        /// Send the command to Rpleth
        /// </summary>
        /// <returns>True if it succesfull or false otherwise</returns>
        public override bool Work(Interface inter, Options option)
        {
            RplethReaderProvider readerProvider = new RplethReaderProvider();
            IRplethReaderUnit readerUnit = readerProvider.CreateReaderUnit() as IRplethReaderUnit;
            IRplethReaderUnitConfiguration readerUnitConfig = readerUnit.Configuration as IRplethReaderUnitConfiguration;

            readerUnitConfig.mode = RplethMode.WIEGAND;
            readerUnitConfig.Length = 0;
            readerUnitConfig.offset = 0;

            IRplethDataTransport dt = readerUnit.DataTransport as IRplethDataTransport;
            dt.IpAddress = TargetIp;
            dt.port = option.TargetPort;

            Console.WriteLine();
            try
            {
                if (readerUnit.ConnectToReader())
                {
                    if (struc.Ip != null)
                    {
                        Console.Write("Send IP: ");
                        readerUnit.SetReaderIp(struc.Ip);
                        Console.WriteLine("DONE.");
                    }

                    if (struc.Mac != null)
                    {
                        Console.Write("Send MAC: ");
                        readerUnit.SetReaderMac(struc.Mac);
                        Console.WriteLine("DONE.");
                    }

                    if (struc.Port != 0)
                    {
                        Console.Write("Send PORT: ");
                        readerUnit.SetReaderPort(struc.Port);
                        Console.WriteLine("DONE.");
                    }

                    if (struc.Subnet != null)
                    {
                        Console.Write("Send SUBNET: ");
                        readerUnit.SetReaderSubnet(struc.Subnet);
                        Console.WriteLine("DONE.");
                    }

                    if (struc.Gateway != null)
                    {
                        Console.Write("Send GATEWAY: ");
                        readerUnit.SetReaderGateway(struc.Gateway);
                        Console.WriteLine("DONE.");
                    }

                    if (struc.Dhcp != 3)
                    {
                        Console.Write("Send DHCP: ");
                        readerUnit.SetDhcpState(Convert.ToBoolean(struc.Dhcp));
                        Console.WriteLine("DONE.");
                    }

                    if (struc.Message != String.Empty)
                    {
                        Console.Write("Send MESSAGE: ");
                        readerUnit.SetReaderDefaultMessage(struc.Message);
                        Console.WriteLine("DONE.");
                    }

                    if (option.reboot == 1)
                    {
                        Console.Write("Send REBOOT: ");
                        readerUnit.ResetReader();
                        Console.WriteLine("DONE.");
                    }

                    readerUnit.DisconnectFromReader();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
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
                inter.WriteMessage("\n*** Rpleth Target Information ***\n");

                while (struc.TargetIp == null)
                {
                    try
                    {
                        if (options.TargetIp == null)
                            struc.TargetIp = inter.GetByteTab("Enter the ip address of Rpleth", 4, '.', false, ref TargetIp);
                        else
                        {
                            struc.TargetIp = StringHelper.StringToByteTab(options.TargetIp, 4, '.');
                            options.TargetIp = null;
                        }
                    }
                    catch (Exception)
                    { }
                }

                while (struc.TargetPort == 0)
                {
                    try
                    {
                        if (options.TargetPort == -1)
                            struc.TargetPort = Convert.ToUInt16(inter.GetUint("Enter the port number that listen the Target Rpleth", false));
                        else
                        {

                            struc.TargetPort = Convert.ToUInt16(options.TargetPort);
                            options.TargetPort = -1;

                        }
                    }
                    catch (Exception)
                    { }

                    options.TargetPort = struc.TargetPort;
                }



                inter.WriteMessage("\n*** Rpleth configuration ***\n");

                if (options.Mac == null)
                    struc.Mac = inter.GetByteTab("Enter the mac address of Rpleth", 6, ':', 16, true);
                else
                {
                    try
                    {
                        struc.Mac = StringHelper.StringToByteTab(options.Mac, 6, ':', 16);
                    }
                    catch (Exception)
                    {
                        struc.Mac = inter.GetByteTab("Enter the mac address of Rpleth", 6, ':', 16, true);
                    }
                }

                if (options.Dhcp == -1)
                    struc.Dhcp = inter.GetByte("Disable the dhcp ?\nEnter 0 to disable or 1 to enable", true);
                else
                    struc.Dhcp = Convert.ToByte(options.Dhcp);


                if (options.Ip == null)
                    struc.Ip = inter.GetByteTab("Enter the ip address of Rpleth", 4, '.', true);
                else
                {
                    try
                    {
                        struc.Ip = StringHelper.StringToByteTab(options.Ip, 4, '.');
                    }
                    catch (Exception)
                    {
                        struc.Ip = inter.GetByteTab("Enter the ip address of Rpleth", 4, '.', true);
                    }
                }

                if (options.Subnet == null)
                    struc.Subnet = inter.GetByteTab("Enter the subnet address for Rpleth", 4, '.', true);
                else
                {
                    try
                    {
                        struc.Subnet = StringHelper.StringToByteTab(options.Subnet, 4, '.');
                    }
                    catch (Exception)
                    {
                        struc.Subnet = inter.GetByteTab("Enter the subnet address for Rpleth", 4, '.', true);
                    }
                }

                if (options.Gateway == null)
                    struc.Gateway = inter.GetByteTab("Enter the gateway address for Rpleth", 4, '.', true);
                else
                {
                    try
                    {
                        struc.Gateway = StringHelper.StringToByteTab(options.Gateway, 4, '.');
                    }
                    catch (Exception)
                    {
                        struc.Gateway = inter.GetByteTab("Enter the gateway address for Rpleth", 4, '.', true);
                    }
                }
      

                if (options.Port == -1)
                    struc.Port = Convert.ToUInt16(inter.GetUint("Enter the port number that listen Rpleth", true));
                else
                    struc.Port = Convert.ToUInt16(options.Port);

                if (options.Welcome == null)
                    struc.Message = inter.GetMessage("Enter the welcome message of Rpleth", true);
                else
                    struc.Message = options.Welcome;

                if (options.reboot == -1)
                    options.reboot = Convert.ToUInt16(inter.GetUint("Do you want to reboot the Rpleth at the end (0/1) ?", true));

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

            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
            byte[] targetip;

            [FieldOffset(54)]
            [MarshalAs(UnmanagedType.U2)]
            ushort targetport;

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

            public byte[] TargetIp
            {
                get { return targetip; }
                set { targetip = value; }
            }

            public ushort TargetPort
            {
                get { return targetport; }
                set { targetport = value; }
            }

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
