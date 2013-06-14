using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpleth
{
    /// <summary>
    /// Represent the differents command of RplethProxy
    /// </summary>
    public class RplethConst
    {
        /// <summary>
        /// Represent the device byte of transmission
        /// </summary>
        public enum Device : byte
        {
            /// <summary>
            /// Administration commands
            /// </summary>
            RPLETH = 0x00,
            /// <summary>
            /// HID Reader commands
            /// </summary>
            HID = 0X01,
            /// <summary>
            /// LCD commands
            /// </summary>
            LCD = 0x02
        }

        /// <summary>
        /// Administration commands
        /// </summary>
        public enum RplethCommand : byte
        {
            /// <summary>
            /// Get the state of Dhcp
            /// </summary>
            STATEDHCP = 0x01,
            /// <summary>
            /// Enable or disable the dhcp
            /// </summary>
            DHCP = 0x02,
            /// <summary>
            /// Change the adress mac of Arduino
            /// </summary>
            MAC = 0x03,
            /// <summary>
            /// Change the arduino ip
            /// </summary>
            IP = 0x04,
            /// <summary>
            /// Change the arduino subnet mask
            /// </summary>
            SUBNET = 0x05,
            /// <summary>
            /// Change the arduino gateway
            /// </summary>
            GATEWAY = 0x06,
            /// <summary>
            /// Change the port of Arduino
            /// </summary>
            PORT = 0x07,
            /// <summary>
            /// Change the Welcome message
            /// </summary>
            MESSAGE = 0x08,
            /// <summary>
            /// Reset Arduino
            /// </summary>
            RESET = 0x09
        }

        /// <summary>
        /// HID Reader commands
        /// </summary>
        public enum HidCommand : byte
        {
            /// <summary>
            /// Make a beep on the speaker 
            /// </summary>
            BIP = 0x00,
            /// <summary>
            /// Blink the led1
            /// </summary>
            BLINKLED1 = 0x01,
            /// <summary>
            /// Blink the led2
            /// </summary>
            BLINKLED2 = 0x02,
            /// <summary>
            /// NOP
            /// </summary>
            NOP = 0x03,
            /// <summary>
            /// Get the number of the badge
            /// </summary>
            BADGE = 0x04,
            /// <summary>
            /// Send a command to the reader
            /// </summary>
            SEND = 0x05
        }

        /// <summary>
        /// LCD commands
        /// </summary>
        public enum LcdCommand : byte
        {
            /// <summary>
            /// Display the message on LCD
            /// </summary>
            DISPLAY = 0x00,
            /// <summary>
            /// Display the message on LCD during a specific time
            /// </summary>
            DISPLAYT = 0X01,
            /// <summary>
            /// Blink the text on LCD
            /// </summary>
            BLINK = 0X02,
            /// <summary>
            /// Scroll the text on LCD
            /// </summary>
            SCROLL = 0X03,
            /// <summary>
            /// Change the display time of message on LCD
            /// </summary>
            DISPLAYTIME = 0x04
        }
    }
}
