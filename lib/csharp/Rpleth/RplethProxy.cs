using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Threading;
using System.Net.NetworkInformation;


/// \mainpage Rpleth Documentation
///
/// \section intro_sec Introduction
///
/// This is the Rpleth documentation.
/// <BR>
/// It's an open software project in Rpleth reader form ISLOG company.
/// 
namespace Rpleth
{
    /// <summary>
    /// Represents a Rpleth ISLOG
    /// </summary>
    public class RplethProxy : INotifyPropertyChanged
    {
        /// <summary>
        /// Used by Entity framework
        /// </summary>
        public Int64 Id { get; set; }
        
        string name;
        /// <summary>
        /// Represents the name of the reader
        /// </summary>
        public string Name
        {
            get { return name; }
            set 
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Represents the ip address of the reader
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// Represents the port of the reader
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Represents the states of waiting process in Badge method
        /// </summary>
        public bool Continues { get; set; }
        
        /// <summary>
        /// Represents the offset needed to do in wiegand frame to get the CSN
        /// </summary>
        public byte Offset { get; set; }
        
        /// <summary>
        /// Represents the lenght of CSN in wiegand frame
        /// </summary>
        public byte Length { get; set; }

        /// <summary>
        /// Represents the transmmission send to teh reader
        /// </summary>
        private byte[] transmission;

        /// <summary>
        /// Represents a instance of TcpClient
        /// </summary>
        private TcpClient tcpclnt = null;

        /// <summary>
        /// Represents the stream use to the communication
        /// </summary>
        private NetworkStream stm;

        private IAsyncResult oldAr;

        /// <summary>
        /// Represents a arduino reader
        /// </summary>
        public RplethProxy()
        {
            Ip = "10.0.0.0";
            Port = 23;
            Name = "Proxy";
            Continues = true;
            Offset = 1;
            Length = 16;
        }

        /// <summary>
        /// Represents a Rpleth reader
        /// </summary>
        /// <param name="p_ip">ip of the reader</param>
        /// <param name="p_port">port of the reader</param>
        public RplethProxy(string p_ip, int p_port)
        {
            Ip = p_ip;
            Port = p_port;
            Name = "Proxy";
            Continues = true;
            Offset = 1;
            Length = 16;
        }

        /// <summary>
        /// Represents a Rpleth reader
        /// </summary>
        /// <param name="p_ip">ip of the reader</param>
        /// <param name="p_port">port of the reader</param>
        /// <param name="p_name">name of the reader</param>
        public RplethProxy(string p_ip, int p_port, string p_name)
        {
            Ip = p_ip;
            Port = p_port;
            Name = p_name;
            Continues = true;
            Offset = 1;
            Length = 16;
        }
        
        /// <summary>
        /// Represents a Rpleth reader
        /// </summary>
        /// <param name="p_ip">ip of the reader</param>
        /// <param name="p_port">port of the reader</param>
        /// <param name="p_name">name of the reader</param>
        /// <param name="p_offset">offset in wiegand trame</param>
        /// <param name="p_length">length of CSN in wiegand trame</param>
        public RplethProxy(string p_ip, int p_port, string p_name, byte p_offset, byte p_length)
        {
            Ip = p_ip;
            Port = p_port;
            Name = p_name;
            Continues = true;
            Offset = p_offset;
            Length = p_length;
        }

        /// <summary>
        /// Get the state of Dhcp
        /// </summary>
        /// <returns>State of dhcp</returns>
        public bool StateDhcp()
        {
            bool result = false;
            transmission = new byte[4];
            transmission[0] = (byte)RplethConst.Device.RPLETH;
            transmission[1] = (byte)RplethConst.RplethCommand.STATEDHCP;
            transmission[2] = 0x00;
            transmission[3] = Compute_checksum();
            Send();
            result = Convert.ToBoolean(Answer()[0]);
            return result;
        }

        /// <summary>
        /// Enable or disable the dhcp
        /// </summary>
        public void ChangeDhcp()
        {
            transmission = new byte[4];
            transmission[0] = (byte)RplethConst.Device.RPLETH;
            transmission[1] = (byte)RplethConst.RplethCommand.DHCP;
            transmission[2] = 0x00;
            transmission[3] = Compute_checksum();
            Send();
            Answer();
        }

        /// <summary>
        /// Change the reader ip
        /// Format XXX.XXX.XXX.XXX
        /// </summary>
        /// <param name="ip">the new reader ip</param>
        public void ChangeIp(string ip)
        {
            string[] ip_split = ip.Split('.');
            if (ip_split.Length != 4)
            {
                throw new RplethException ("Bad IP, format XXX.XXX.XXX.XXX");
            }
            transmission = new byte[8];
            transmission[0] = (byte)RplethConst.Device.RPLETH;
            transmission[1] = (byte)RplethConst.RplethCommand.IP;
            transmission[2] = 4;
            try
            {
                for (int i = 0; i < ip_split.Length; i++)
                {
                    transmission[3 + i] = Convert.ToByte(ip_split[i]);
                }
            }
            catch (RplethException e)
            {
                throw e;
            }
            transmission[7] = Compute_checksum();
            Send();
            Answer();
        }

        /// <summary>
        /// Change the reader subnet mask
        /// Format XXX.XXX.XXX.XXX
        /// </summary>
        /// <param name="subnet">the new reader subnet mask</param>
        public void ChangeSubnet(string subnet)
        {
            string[] subnet_split = subnet.Split('.');
            if (subnet_split.Length != 4)
            {
                throw new RplethException("Bad SUBNET, format XXX.XXX.XXX.XXX");
            }
            transmission = new byte[8];
            transmission[0] = (byte)RplethConst.Device.RPLETH;
            transmission[1] = (byte)RplethConst.RplethCommand.SUBNET;
            transmission[2] = 0x04;
            try
            {
                for (int i = 0; i < subnet_split.Length; i++)
                {
                    transmission[3 + i] = Convert.ToByte(subnet_split[i]);
                }

            }
            catch (RplethException e)
            {
                throw e;
            }
            transmission[7] = Compute_checksum();
            Send();
            Answer();
        }

        /// <summary>
        /// Change the reader gateway
        /// Format XXX.XXX.XXX.XXX
        /// </summary>
        /// <param name="gateway">the new reader gateway</param>
        public void ChangeGateway(string gateway)
        {
            string[] gateway_split = gateway.Split('.');
            if (gateway_split.Length != 4)
            {
                throw new RplethException("Bad GATEWAY, format XXX.XXX.XXX.XXX");
            }
            transmission = new byte[8];
            transmission[0] = (byte)RplethConst.Device.RPLETH;
            transmission[1] = (byte)RplethConst.RplethCommand.GATEWAY;
            transmission[2] = 0x04;
            try
            {
                for (int i = 0; i < gateway_split.Length; i++)
                {
                    transmission[3 + i] = Convert.ToByte(gateway_split[i]);
                }

            }
            catch (RplethException e)
            {
                throw e;
            }
            transmission[7] = Compute_checksum();
            Send();
            Answer();
        }

        /// <summary>
        /// Change the port of reader
        /// </summary>
        /// <param name="port">The new port</param>
        public void ChangePort(int port)
        {
            transmission = new byte[6];
            transmission[0] = (byte)RplethConst.Device.RPLETH;
            transmission[1] = (byte)RplethConst.RplethCommand.PORT;
            transmission[2] = 0x02;
            transmission[3] = Convert.ToByte(port >> 8);
            transmission[4] = Convert.ToByte(port & 0xff);
            transmission[5] = Compute_checksum();
            Send();
            Answer();
        }

        /// <summary>
        /// Reset the reader
        /// </summary>
        public void Reset()
        {
            transmission = new byte[4];
            transmission[0] = (byte)RplethConst.Device.RPLETH;
            transmission[1] = (byte)RplethConst.RplethCommand.RESET;
            transmission[2] = 0x00;
            transmission[3] = Compute_checksum();
            Send();
            Answer();
        }

        /// <summary>
        /// Change the reader mac of Arduino
        /// Format XX.XX.XX.XX.XX.XX
        /// </summary>
        /// <param name="mac">The new adress mac</param>
        public void ChangeMac(string mac)
        {
            string[] mac_split = mac.Split('.');
            if (mac_split.Length != 6)
                throw new RplethException("Bad MAC, format XX.XX.XX.XX.XX.XX");
            transmission = new byte[10];
            transmission[0] = (byte)RplethConst.Device.RPLETH;
            transmission[1] = (byte)RplethConst.RplethCommand.MAC;
            transmission[2] = 0x06;
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    transmission[3 + i] = Convert.ToByte(mac_split[i], 16);
                }
            }
            catch (RplethException e)
            {
                throw e;
            }
            transmission[9] = Compute_checksum();
            Send();
            Answer();
        }

        /// <summary>
        /// Change the Welcome message
        /// </summary>
        /// <param name="message">The new welcome messag</param>
        public void ChangeWelcomeMessage(string message)
        {
            transmission = new byte [4+message.Length];
            transmission[0] = (byte)RplethConst.Device.RPLETH;
            transmission[1] = (byte)RplethConst.RplethCommand.MESSAGE;
            transmission[2] = Convert.ToByte(message.Length);
            try
            {
                for (int i = 0; i < message.Length; i++)
                {
                    transmission[3 + i] = Convert.ToByte(message[i]);
                }
            }
            catch (RplethException e)
            {
                throw e;
            }
            transmission[3 + message.Length] = Compute_checksum();
            Send();
            Answer();
        }

        /// <summary>
        /// Make a beep on the speaker 
        /// </summary>
        /// <param name="time">Time of the beep</param>
        public void Beep(byte time)
        {
            transmission = new byte[5];
            transmission[0] = (byte)RplethConst.Device.HID;
            transmission[1] = (byte)RplethConst.HidCommand.BIP;
            transmission[2] = 0x01;
            transmission[3] = time;
            transmission[4] = Compute_checksum();
            Send();
            Answer();
        }

        /// <summary>
        /// Blink the led1
        /// </summary>
        /// <param name="time">Time of blinking</param>
        public void BlinkLed1(byte time)
        {
            transmission = new byte[5];
            transmission[0] = (byte)RplethConst.Device.HID;
            transmission[1] = (byte)RplethConst.HidCommand.BLINKLED1;
            transmission[2] = 0x01;
            transmission[3] = time;
            transmission[4] = Compute_checksum();
            Send();
            Answer();
        }

        /// <summary>
        /// Blink the led2
        /// </summary>
        /// <param name="time">Time of blinking</param>
        public void BlinkLed2(byte time)
        {
            transmission = new byte[5];
            transmission[0] = (byte)RplethConst.Device.HID;
            transmission[1] = (byte)RplethConst.HidCommand.BLINKLED2;
            transmission[2] = 0x01;
            transmission[3] = time;
            transmission[4] = Compute_checksum();
            Send();
            Answer();
        }

        /// <summary>
        /// NOP
        /// </summary>
        public void Nop()
        {
            transmission = new byte[4];
            transmission[0] = (byte)RplethConst.Device.HID;
            transmission[1] = (byte)RplethConst.HidCommand.NOP;
            transmission[2] = 0x00;
            transmission[3] = Compute_checksum();
            Send();
            Answer();
        }

        /// <summary>
        /// Get the number of the badge wait until Continues is false
        /// </summary>
        /// <returns>The badge</returns>
        public ulong Badge()
        {
            ulong result = 0;
            transmission = new byte[4];
            transmission[0] = (byte)RplethConst.Device.HID;
            transmission[1] = (byte)RplethConst.HidCommand.BADGE;
            transmission[2] = 0x00;
            transmission[3] = Compute_checksum();
            Send();
            Answer();
            result = GetBadge();
            return result;
        }

        /// <summary>
        /// Get the number of a badge
        /// </summary>
        /// <param name="timeout">The time to wait</param>
        /// <returns>The badge</returns>
        public ulong Badge(int timeout)
        {
            ulong result = 0;
            transmission = new byte[4];
            transmission[0] = (byte)RplethConst.Device.HID;
            transmission[1] = (byte)RplethConst.HidCommand.BADGE;
            transmission[2] = 0x00;
            transmission[3] = Compute_checksum();
            Send();
            Answer();
            result = GetBadge(timeout);
            return result;
        }

        /// <summary>
        /// Blink the text on LCD
        /// </summary>
        public void BlinkLcd()
        {
            transmission = new byte[4];
            transmission[0] = (byte)RplethConst.Device.LCD;
            transmission[1] = (byte)RplethConst.LcdCommand.BLINK;
            transmission[2] = 0x00;
            transmission[3] = Compute_checksum();
            Send();
            Answer();
        }

         /// <summary>
         /// Scroll the text on LCD
         /// </summary>
        public void ScrollLcd()
        {
            transmission = new byte[4];
            transmission[0] = (byte)RplethConst.Device.LCD;
            transmission[1] = (byte)RplethConst.LcdCommand.SCROLL;
            transmission[2] = 0x00;
            transmission[3] = Compute_checksum();
            Send();
            Answer();
        }

        /// <summary>
        /// Change the display time of message on LCD
        /// </summary>
        /// <param name="time">display time</param>
        public void ChangeDisplayTimeLcd(byte time)
        {
            transmission = new byte[5];
            transmission[0] = (byte)RplethConst.Device.LCD;
            transmission[1] = (byte)RplethConst.LcdCommand.DISPLAYTIME;
            transmission[2] = 0x01;
            transmission[3] = time;
            transmission[4] = Compute_checksum();
            Send();
            Answer();
        }

        /// <summary>
        /// Display the message on LCD
        /// </summary>
        /// <param name="message">message to display</param>
        public void DisplayLcd(string message)
        {
            transmission = new byte[4 + message.Length];
            transmission[0] = (byte)RplethConst.Device.LCD;
            transmission[1] = (byte)RplethConst.LcdCommand.DISPLAY;
            transmission[2] = Convert.ToByte (message.Length);
            try
            {
                for (int i = 0; i < message.Length; i++)
                {
                    transmission[3 + i] = Convert.ToByte(message[i]);
                }
            }
            catch (RplethException e)
            {
                throw e;
            }
            transmission[message.Length + 3] = Compute_checksum();
            Send();
            Answer();
        }

        /// <summary>
        /// Display the message on LCD  during a specific time
        /// </summary>
        /// <param name="message">message to display</param>
        /// <param name="time">time to display(-1 to infini)</param>
        public void DisplayLcd(string message, int time)
        {
            transmission = new byte[5 + message.Length];
            transmission[0] = (byte)RplethConst.Device.LCD;
            transmission[1] = (byte)RplethConst.LcdCommand.DISPLAYT;
            transmission[2] = Convert.ToByte(message.Length+1);
            try
            {
                for (int i = 0; i < message.Length; i++)
                {
                    transmission[3 + i] = Convert.ToByte(message[i]);
                }
            }
            catch (RplethException e)
            {
                throw e;
            }
            transmission[message.Length + 3] = Convert.ToByte(time);
            transmission[message.Length + 4] = Compute_checksum();
            Send();
            Answer();
        }

        /// <summary>
        /// Send a command
        /// </summary>
        /// <param name="command">the command to send</param>
        /// <returns>the answer of the command send</returns>
        public string SendCommand(string command)
        {
            string result = "";
            byte[] answer;
            char[] strAnswer;
            transmission = new byte[4 + command.Length];
            transmission[0] = (byte)RplethConst.Device.HID;
            transmission[1] = (byte)RplethConst.HidCommand.SEND;
            transmission[2] = Convert.ToByte(command.Length);
            try
            {
                for (int i = 0; i < command.Length; i++)
                {
                    transmission[3 + i] = Convert.ToByte(command[i]);
                }
            }
            catch (RplethException e)
            {
                throw e;
            }
            transmission[command.Length + 3] = Compute_checksum();
            Send();
            answer = Answer();
            strAnswer = new char[answer.Length];
            for (int i = 0; i < answer.Length; i++)
            {
                strAnswer[i] = Convert.ToChar(answer[i]);
            }
            result = new string (strAnswer);
            return result;
        }

        /// <summary>
        /// Connect to Rpleth
        /// </summary>
        public void Connect()
        {
            try
            {
                tcpclnt = new TcpClient();
                AutoResetEvent connectDone = new AutoResetEvent(false);
                connectDone.Reset();
                oldAr = tcpclnt.BeginConnect(
                    Ip, Port,
                    new AsyncCallback(
                        delegate (IAsyncResult ar)
                        {
                            try
                            {
                                if (ar == oldAr)
                                    tcpclnt.EndConnect(ar);
                                connectDone.Set();
                            }
                            catch
                            {

                            }
                        }
                    ), tcpclnt
                );
                while (!connectDone.WaitOne(1000) && Continues)
                {

                }
                if (Continues)
                {
                    stm = tcpclnt.GetStream();
                    stm.ReadTimeout = 1000;
                }
                else
                    throw new Exception("Not Connected");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Return the state of connection
        /// </summary>
        /// <returns></returns>
        public bool Connected()
        {
            if (tcpclnt.Client != null)
                return tcpclnt.Connected;
            else
                return false;
        }

        /// <summary>
        /// Disconnect form Rpleth
        /// </summary>
        public void Disconnect()
        {
            tcpclnt.Close();
        }

        /// <summary>
        /// Compute the checksum of the transmission
        /// </summary>
        /// <returns>the checksum</returns>
        private byte Compute_checksum ()
        {
            byte result = 0;
            for (int i = 0; i < transmission.Length-1; i++)
            {
                result ^= transmission[i];
            }
            return result;
        }

        /// <summary>
        /// Send the transmission on the 
        /// </summary>
        private void Send ()
        {
            stm.Write(transmission, 0, transmission.Length);
        }

        /// <summary>
        /// Get the Answer of Rpleth
        /// </summary>
        /// <returns>data when it's needed</returns>
        private byte [] Answer()
        {
            byte[] result;
            int answer;
            byte checksum = 0;
            byte checksum_dist = 0;
            byte size = 0;
            byte[] cmd_byte = new byte[3];
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    answer = stm.ReadByte();
                    checksum ^= Convert.ToByte(answer);
                    cmd_byte[i] = Convert.ToByte(answer);
                }
            }
            catch (RplethException e)
            {
                throw e;
            }
            if (cmd_byte[1] != transmission[0] || cmd_byte[2] != transmission[1])
            {
                stm.Flush();
                return Answer();
            }
            answer = stm.ReadByte();
            checksum ^= Convert.ToByte(answer);
            size = Convert.ToByte(answer);
            result = new byte [size];
            for (int i = 0; i < size; i++)
            {
                result[i] = Convert.ToByte(stm.ReadByte());
                checksum ^= result[i];
            }
            checksum_dist = Convert.ToByte(stm.ReadByte());
            if (checksum != checksum_dist)
            {
                throw new RplethException ("Bad checksum in answer");
            }
            else if (cmd_byte[0] == 0x01)
            {
                throw new RplethException("Error in the command");
            }
            else if (cmd_byte[0] == 0x02)
            {
                throw new RplethException("Bad checksum in command");
            }
            else if (cmd_byte[0] == 0x03)
            {
                throw new TimeoutException ();
            }
            else if (cmd_byte[0] == 0x04)
            {
                throw new RplethException("Bad lenght in command");
            }
            else if (cmd_byte[0] == 0x05)
            {
                throw new RplethException("Bad device type in command");
            }
            return result;
        }

        /// <summary>
        /// Get the number of a badge while Continues == false
        /// </summary>
        /// <returns>the number of a badge</returns>
        private ulong GetBadge()
        {
            byte checksum = 0;
            byte checksum_dst = 0;
            int answer = 0;
            ulong result = 0;
            byte[] com = new byte[4];
            while (!stm.DataAvailable && Continues) { }
            if (Continues)
            {
                for (int i = 0; i < 4; i++)
                {
                    answer = stm.ReadByte();
                    checksum ^= Convert.ToByte(answer);
                    com[i] = Convert.ToByte(answer);
                }
                if (com[1] != 0x01 && com[2] != 0x04)
                {
                    stm.Flush();
                    return GetBadge();
                }
                else
                {
                    result = 0;
                    for (int j = 0; j < com[3]; j++)
                    {
                        result <<= 8;
                        result |= Convert.ToByte(stm.ReadByte());
                        checksum ^= Convert.ToByte(result & 0xff);
                    }
                    checksum_dst = Convert.ToByte(stm.ReadByte());
                    if (checksum != checksum_dst)
                    {
                        throw new RplethException("Bad Checksum in answer");
                    }
                    result = GetCSN (result);
                }
            }
            return result;
        }

        /// <summary>
        /// Get the number of a badge while p_timeout
        /// </summary>
        /// <param name="p_timeout">the time to wait a badge</param>
        /// <returns>the number of a badge</returns>
        private ulong GetBadge(int p_timeout)
        {
            byte checksum = 0;
            byte checksum_dst = 0;
            int answer = 0;
            ulong result = 0;
            byte[] com = new byte[4];
            DateTime dtStart = DateTime.Now;
            TimeSpan timeout = DateTime.Now.Subtract(dtStart);
            while (!stm.DataAvailable && timeout.Seconds != p_timeout && Continues) { }
            if (timeout.Seconds != p_timeout && Continues)
            {
                for (int i = 0; i < 4; i++)
                {
                    answer = stm.ReadByte();
                    checksum ^= Convert.ToByte(answer);
                    com[i] = Convert.ToByte(answer);
                }
                if (com[1] != 0x01 && com[2] != 0x04)
                {
                    stm.Flush();
                    return GetBadge(p_timeout);
                }
                else
                {
                    result = 0;
                    for (int j = 0; j < com[3]; j++)
                    {
                        result <<= 8;
                        result |= Convert.ToByte(stm.ReadByte());
                        checksum ^= Convert.ToByte(result & 0xff);
                    }
                    checksum_dst = Convert.ToByte(stm.ReadByte());
                    if (checksum != checksum_dst)
                    {
                        throw new RplethException("Bad Checksum in answer");
                    }
                    result = GetCSN (result);
                }
            }
            else if (Continues)
            {
                throw new TimeoutException ();
            }
            return result;
        }
        
        /// <summary>
        /// Get the csn of a badge in wiegand trame
        /// </summary>
        /// <param name="trame">the full wiegand trame</param>
        /// <returns>the csn of a badge</returns>
        public ulong GetCSN (ulong trame)
        {
			ulong result = 0;
			result = (trame >> Offset) & (ulong)(Math.Pow(2, Length)-1);
			return result;
		}

        /// <summary>
        /// Event trigger when a property has change
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Method call when a property has change
        /// </summary>
        /// <param name="propertyName">name of the property</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
