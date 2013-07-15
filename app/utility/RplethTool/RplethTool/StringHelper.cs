using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RplethTool
{
    /// <summary>
    /// This object provide some string convertion method very usefull.
    /// </summary>
	class StringHelper
	{
        /// <summary>
        /// Constructor
        /// </summary>
		public StringHelper() { }

        /// <summary>
        /// Convert a string into a byte tab. The string is splited with separator and this spleting must have the right size.
        /// </summary>
        /// <param name="message">The string to convert</param>
        /// <param name="size">The size of the byte tab</param>
        /// <param name="separator">The char use to split the string</param>
        /// <param name="numberBase">The base number of any byte</param>
        /// <returns>The converted byte tab</returns>
		public static byte[] StringToByteTab (string message, int size, char separator, int numberBase)
		{
			byte[] res = null;
			try 
			{
				string[] messageSpleted = message.Split(separator);
				if (messageSpleted.Length != size)
					throw new Exception("Error : Bad message parsing");
				res = new byte[size];
				for (int i = 0; i < size; i++)
				{
                    res[i] = Convert.ToByte(messageSpleted[i], numberBase);
				}
			}
			catch (Exception e)
			{
				throw e;
			}
			return res;
		}

        /// <summary>
        /// Convert a string into a byte tab. The string is splited with separator and this spleting must have the right size. Use base 10.
        /// </summary>
        /// <param name="message">The string to convert</param>
        /// <param name="size">The size of the byte tab</param>
        /// <param name="separator">The char use to split the string</param>
        /// <returns>The converted byte tab</returns>
        public static byte[] StringToByteTab(string message, int size, char separator)
        {
            return StringToByteTab(message, size, separator, 10);
        }

	}
}
