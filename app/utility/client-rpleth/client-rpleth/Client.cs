using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Rpleth;
using CommandLine;
using CommandLine.Text;

namespace Client_tpc_rpleth
{
    class Client
    {
        public class Options
        {
            [Option('i', "ip", DefaultValue = "0.0.0.0", HelpText = "Ip of the Rpleth.")]
            public string Ip { get; set; }

            [Option('p', "port", DefaultValue = 0, HelpText = "Port of the Rpleth.")]
            public int Port { get; set; }

            [Option('m', "message", DefaultValue = null, HelpText = "Data(string) to send.")]
            public string Message { get; set; }

            [Option('d', "data", DefaultValue = -1, HelpText = "Data(int) to send.")]
            public int Time { get; set; }

            [Option('c', "command", DefaultValue = -1, HelpText = "The command you ask to the Rpleth.")]
            public int Command { get; set; }

            [Option('o', "offset", DefaultValue = 1, HelpText = "The offset in tram of wiegand communication.")]
            public int Offset { get; set; }

            [Option('l', "lenght", DefaultValue = 16, HelpText = "The lenght of number in tram of wiegand communication.")]
            public int Lenght { get; set; }

            [Option('r', "tramsize", DefaultValue = 26, HelpText = "The size of a tram in wiegand communication.")]
            public int TramSize { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this,
                current => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }

        public static byte GetCmd()
        {
            byte[] cmd_byte = new byte [3];
            byte cmd;
            string tmp;
            Console.WriteLine("Type the code");
            Console.WriteLine("Rpleth adiministration :");
            Console.WriteLine("  0 to get the state of the dhcp");
            Console.WriteLine("  1 to enable/disable the dhcp");
            Console.WriteLine("  2 to change the mac of Rpleth");
            Console.WriteLine("  3 to change the ip of Rpleth");
            Console.WriteLine("  4 to change the subnet of Rpleth");
            Console.WriteLine("  5 to change the gateway of Rpleth");
            Console.WriteLine("  6 to change the port of Rpleth");
            Console.WriteLine("  7 to change the welcome message");
            Console.WriteLine("  8 to change Wiengand configuration");
            Console.WriteLine("  9 to reset Rpleth");
            Console.WriteLine("HID Command");
            Console.WriteLine("  10 to make a bip");
            Console.WriteLine("  11 to make blink the led1");
            Console.WriteLine("  12 to make blink the led2");
            Console.WriteLine("  13 to NOP");
            Console.WriteLine("  14 to get a badge");
            Console.WriteLine("  20 to send a command");
            Console.WriteLine("LCD Command");
            Console.WriteLine("  15 to write on the lcd");
            Console.WriteLine("  16 to write on the lcd (specifying the display time)");
            Console.WriteLine("  17 to blink the lcd");
            Console.WriteLine("  18 to scroll the lcd");
            Console.WriteLine("  19 to change display time on the lcd");
            tmp = Console.ReadLine();
            cmd = Convert.ToByte(tmp);
            return cmd;
        }

        public static byte GetSize(int cmd)
        {
            byte size = 0;
            string tmp;
            switch (cmd)
            {
                case 10:
                    Console.WriteLine("bip time ?");
                    tmp = Console.ReadLine();
                    size = Convert.ToByte(tmp);
                    break;
                case 11:
                    Console.WriteLine("blink time ?");
                    tmp = Console.ReadLine();
                    size = Convert.ToByte(tmp);
                    break;
                case 12:
                    Console.WriteLine("blink time ?");
                    tmp = Console.ReadLine();
                    size = Convert.ToByte(tmp);
                    break;
                case 14:
                    Console.WriteLine("Timeout ?(enter to default 30s)");
                    tmp = Console.ReadLine();
                    if (tmp.Length != 0)
                    {
                        size = Convert.ToByte(tmp);
                    }
                    break;
                case 16:
                    Console.WriteLine("display time ?(enter to infinty)");
                    tmp = Console.ReadLine();
                    if (tmp.Length != 0)
                    {
                        size = Convert.ToByte(tmp);
                    }
                    break;
                case 19:
                    Console.WriteLine("new display time ?");
                    tmp = Console.ReadLine();
                    size = Convert.ToByte(tmp);
                    break;
            }
            return size;
        }

        public static string GetMessage()
        {
            string result = null;
            try
            {
                Console.WriteLine("Enter a message");
                result = Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
            return result;
        }

        public static string GetIp()
        {
            Console.WriteLine("Enter the IP address of Rpleth");
            return Console.ReadLine();
        }

        public static int GetPort()
        {
            Console.WriteLine("Enter the Port of Rpleth");
            return Convert.ToInt32(Console.ReadLine());
        }


        public static int GetNewPort()
        {
            Console.WriteLine("Enter new the Port of Rpleth");
            return Convert.ToInt32(Console.ReadLine());
        }

        public static Options GetWiegand(Options op)
        {
            Console.WriteLine("Wiegand Conf:");
            string tmp;
            if (op.Lenght == 16)
            {
                Console.WriteLine("new lenght ? (enter to default)");
                tmp = Console.ReadLine();
                if (tmp.Length != 0)
                {
                    op.Lenght = Convert.ToByte(tmp);
                }
            }
            if (op.Offset == 1)
            {
                Console.WriteLine("new offset ? (enter to default)");
                tmp = Console.ReadLine();
                if (tmp.Length != 0)
                {
                    op.Offset = Convert.ToByte(tmp);
                }
            }
            if (op.TramSize == 26)
            {
                Console.WriteLine("new tram size ? (enter to default)");
                tmp = Console.ReadLine();
                if (tmp.Length != 0)
                {
                    op.TramSize = Convert.ToByte(tmp);
                }
            }
            return op;
        }

        public static string GetNewIp()
        {
            string result = null;
            try
            {
                Console.WriteLine("Enter the new ip");
                result = Console.ReadLine();
                if (result.Split('.').Length != 4)
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
            return result;
        }

        public static void Command(RplethProxy ard, Options op)
        {
            byte size = Convert.ToByte(op.Time);
            if (op.Command < 21)
            {
                ard.Connect();
                switch (op.Command)
                {
                    case 0:
                        Console.WriteLine(ard.StateDhcp());
                        break;
                    case 1:
                        ard.ChangeDhcp();
                        break;
                    case 2:
                        ard.ChangeMac(op.Message);
                        break;
                    case 3:
                        ard.ChangeIp(op.Message);
                        break;
                    case 4:
                        ard.ChangeSubnet(op.Message);
                        break;
                    case 5:
                        ard.ChangeGateway(op.Message);
                        break;
                    case 6:
                        ard.ChangePort(op.Time);
                        break;
                    case 7:
                        ard.ChangeWelcomeMessage(op.Message);
                        break;
                    case 8:
                        ard.ChangeConfWiegand(Convert.ToByte(op.Offset), Convert.ToByte(op.Lenght), Convert.ToByte(op.TramSize));
                        break;
                    case 9:
                        ard.Reset();
                        break;
                    case 10:
                        ard.Beep(size);
                        break;
                    case 11:
                        ard.BlinkLed1(size);
                        break;
                    case 12:
                        ard.BlinkLed1(size);
                        break;
                    case 13:
                        ard.Nop();
                        break;
                    case 14:
                        if (op.Time != 0)
                            Console.WriteLine(ard.Badge(op.Time));
                        else
                            Console.WriteLine(ard.Badge());
                        break;
                    case 15:
                        ard.DisplayLcd(op.Message);
                        break;
                    case 16:
                        ard.DisplayLcd(op.Message, op.Time);
                        break;
                    case 17:
                        ard.BlinkLcd();
                        break;
                    case 18:
                        ard.ScrollLcd();
                        break;
                    case 19:
                        ard.ChangeDisplayTimeLcd(size);
                        break;
                    case 20:
                        Console.Write(ard.SendCommand(op.Message));
                        break;
                }
                ard.Disconnect();
            }
            else
            {
                Console.WriteLine("Bad command");
            }
        }

        static void Main(string[] args)
        {
            var options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Environment.Exit(0);
            }
            if (options.Ip.CompareTo("0.0.0.0") == 0)
            {
                options.Ip = GetIp();
            }
            if (options.Port == 0)
            {
                options.Port = GetPort();
            }
            RplethProxy ard = new RplethProxy(options.Ip, options.Port);
            if (options.Command == -1)
            {
                options.Command = GetCmd();
            }
            if (options.Time == -1)
            {
                options.Time = GetSize(options.Command);
            }
            if (options.Message == null && (options.Command == 15 || options.Command == 16 || options.Command == 7))
            {
                options.Message = GetMessage();
            }
            if (options.Command == 8 && options.Lenght == 16 && options.Offset == 1 && options.TramSize == 26)
            {
                options = GetWiegand(options);
            }
            if (options.Command == 6 && options.Time == -1)
            {
                options.Time = GetNewPort();
            }
            if (options.Command > 2 && options.Command < 6 && options.Message == null)
            {
                options.Message = GetNewIp();
            }
            Command(ard, options);
        }
    }
}
