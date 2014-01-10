using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace RplethTool
{
    /// <summary>
    /// The interface class. This is an object who provide all I/O methods.
    /// </summary>
    public class Interface
    {
        /// <summary>
        /// The list of Configuration.
        /// </summary>
        public Configuration.ConfigurationList confList = new Configuration.ConfigurationList();

        /// <summary>
        /// Constructor
        /// </summary>
        public Interface()
        {}

        /// <summary>
        /// Display a welcome message.
        /// </summary>
        public void Welcome()
        {
            Console.WriteLine("\t\t\t***********************");
            Console.WriteLine("\t\t\tWelcome to RplethTool.");
            Console.WriteLine("\t\t\t***********************");
            Console.WriteLine("\nThis tool provide you a simple way to configure your Rpleth.\n");
        }

        /// <summary>
        /// Display a GoodBye message.
        /// </summary>
        public void GoodBye()
        {
            Console.WriteLine("\t\t\t***********************");
            Console.WriteLine("\t\t\tThanks to use our Tool.");
            Console.WriteLine("\t\t\t\tGoodBy.");
            Console.WriteLine("\t\t\t***********************");
        }

        /// <summary>
        /// Display a menu
        /// </summary>
        /// <returns>The number selected or throw an exception</returns>
        public int GetMenu()
        {
            int result = 0;
            string tmp;
            Console.WriteLine("Enter the number corresponding to your Rpleth.\n");
            for (int i = 0; i < confList.ConfigurationsList.Count; i++)
            {
                tmp = "\t";
                tmp += i;
                tmp += " to ";
                tmp += confList.ConfigurationsList[i].ToString();
                tmp += ".";
                Console.WriteLine(tmp);
            }
            tmp = "\t";
            tmp += confList.ConfigurationsList.Count;
            tmp += " to quit.";
            Console.WriteLine(tmp);
            Console.Write("\n\t");

            tmp = Console.ReadLine();
            try
            {
                result = Convert.ToInt32(tmp);
                if (result > confList.ConfigurationsList.Count)
                {
                    throw (new Exception("Error : Bad writing number"));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        /// <summary>
        /// Display the message and ask a integer to the user.
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="numberBase">The base of the number writing by the user</param>
        /// <returns>The integer ask to the user</returns>
        public int GetInt(string message, int numberBase)
        {
            int res = 0;
            try
            {
                string tmp;
                if (message != string.Empty)
                    Console.WriteLine(message);
                else
                    Console.WriteLine("Enter a number or ENTER to pass");
                Console.Write("\t");
                tmp = Console.ReadLine();
                if (tmp != String.Empty)
                    res = Convert.ToInt32(tmp, numberBase);
            }
            catch (Exception e)
            {
                throw e;
            }
            return res;
        }

        /// <summary>
        /// Display the message and ask a integer to the user. Use base 10.
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <returns>The integer ask to the user</returns>
        public int GetInt(string message)
        {
            return GetInt(message, 10);
        }

        /// <summary>
        /// Display the message and ask a unsigned integer to the user.
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="numberBase">The base of the number writing by the user</param>
        /// <param name="optional">The argument is optional</param>
        /// <returns>The unsigned integer ask to the user</returns>
        public uint GetUint(string message, int numberBase, bool optional)
        {
            UInt32 res = 0;
            try
            {
                string tmp, opt = "";
                if (optional)
                    opt = " or ENTER to pass";
                Console.WriteLine(message + opt);
                Console.Write("\t");
                tmp = Console.ReadLine();
                if (tmp != String.Empty)
                    res = Convert.ToUInt32(tmp, numberBase);
            }
            catch (Exception e)
            {
                throw e;
            }
            return res;
        }

        /// <summary>
        /// Display the message and ask a unsigned integer to the user. Use base 10.
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="optional">The argument is optional</param>
        /// <returns>The unsigned integer ask to the user</returns>
        public uint GetUint(string message, bool optional)
        {
            return GetUint(message, 10, optional);
        }

        /// <summary>
        /// Display the message and ask a long to the user.
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="numberBase">The base of the number writing by the user</param>
        /// <returns>The long ask to the user</returns>
        public long GetLong(string message, int numberBase)
        {
            long res = 0;
            try
            {
                string tmp;
                if (message != string.Empty)
                    Console.WriteLine(message);
                else
                    Console.WriteLine("Enter a number or ENTER to pass");
                Console.Write("\t");
                tmp = Console.ReadLine();
                if (tmp != String.Empty)
                    res = Convert.ToInt64(tmp, numberBase);
            }
            catch (Exception e)
            {
                throw e;
            }
            return res;
        }

        /// <summary>
        /// Display the message and ask a long to the user. Use base 10.
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <returns>The long ask to the user</returns>
        public long GetLong(string message)
        {
            return GetLong(message, 10);
        }

        /// <summary>
        /// Display the message and ask tab to the user.
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="size">The size of the tab</param>
        /// <param name="separator">The separor to use to split the string receive</param>
        /// <param name="numberBase">The base of the number writing by the user</param>
        /// <param name="optional">The argument is optional</param>
        /// <param name="tmp">The result of the command as string</param>
        /// <returns>The tab ask to the user</returns>
        public byte[] GetByteTab(string message, int size, char separator, int numberBase, bool optional, ref string tmp)
        {
            byte[] result = null;
            if (size > 0 && (numberBase == 2 || numberBase == 10 || numberBase == 16))
            {
                result = null;
                try
                {
                    tmp = null;
                    string format = "(Format: ";
                    for (int i = 0; i < size; i++)
                    {
                        if (numberBase == 10)
                            format += "XXX";
                        else if (numberBase == 16)
                            format += "XX";
                        else if (numberBase == 2)
                            format += "X";
                        if (i != size - 1)
                            format += separator;
                    }
                    format += ")";
                    if (message != string.Empty)
                        Console.WriteLine(message);
                    else
                        Console.WriteLine("Enter an address");
                    string opt = "";
                    if (optional)
                        opt = " or ENTER to pass";
                    Console.WriteLine(format + opt);

                    Console.Write("\t");
                    tmp = Console.ReadLine();
                    if (tmp != String.Empty || !optional)
                        result = StringHelper.StringToByteTab(tmp, size, separator, numberBase);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return result;
        }

        /// <summary>
        /// Display the message and ask a tab to the user. Use base 10.
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="size">The size of the tab</param>
        /// <param name="separator">The separor to use to split the string receive</param>
        /// <param name="optional">The argument is optional</param>
        /// <param name="result">The result of the command as string</param>
        /// <returns>The tab ask to the user</returns>
        public byte[] GetByteTab(string message, int size, char separator, bool optional, ref string result)
        {
            return GetByteTab(message, size, separator, 10, optional, ref result);
        }

        /// <summary>
        /// Display the message and ask a tab to the user. Use base 10.
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="size">The size of the tab</param>
        /// <param name="separator">The separor to use to split the string receive</param>
        /// <param name="optional">The argument is optional</param>
        /// <returns>The tab ask to the user</returns>
        public byte[] GetByteTab(string message, int size, char separator, bool optional)
        {
            return GetByteTab(message, size, separator, 10, optional);
        }

        /// <summary>
        /// Display the message and ask a tab to the user. Use base 10.
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="size">The size of the tab</param>
        /// <param name="separator">The separor to use to split the string receive</param>
        /// <param name="numberBase">The base of the number writing by the user</param>
        /// <param name="optional">The argument is optional</param>
        /// <returns>The tab ask to the user</returns>
        public byte[] GetByteTab(string message, int size, char separator, int numberBase, bool optional)
        {
            string tmp = null;
            return GetByteTab(message, size, separator, numberBase, optional, ref tmp);
        }

        /// <summary>
        /// Display the message and ask string to the user.
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="optional">The argument is optional</param>
        /// <returns>The string ask to the user</returns>
        public string GetMessage(string message, bool optional)
        {
            string res;
            try
            {
                string opt = "";
                if (optional)
                    opt = " or ENTER to pass";
                Console.WriteLine(message + opt);
                Console.Write("\t");
                res = Console.ReadLine();
                if (res != string.Empty || !optional)
                    throw new Exception("Error : Bad string receive");
            }
            catch (Exception e)
            {
                throw e;
            }
            return res;
        }

        /// <summary>
        /// Display the message and ask a byte to the user.
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="numberBase">The base of the number writing by the user</param>
        /// <param name="optional">The argument is optional</param>
        /// <returns>The byte ask to the user</returns>
        public byte GetByte(string message, int numberBase, bool optional)
        {
            byte res = 3;
            try
            {
                string tmp;
                string opt = "";
                if (optional)
                    opt = " or ENTER to pass";
                Console.WriteLine(message + opt);
                Console.Write("\t");
                tmp = Console.ReadLine();
                if (tmp != String.Empty || !optional)
                    res = Convert.ToByte(tmp, numberBase);
            }
            catch (Exception e)
            {
                throw e;
            }
            return res;
        }

        /// <summary>
        /// Display the message and ask a byte to the user. Use base 10.
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="optional">The argument is optional</param>
        /// <returns>The byte ask to the user</returns>
        public byte GetByte(string message, bool optional)
        {
            return GetByte(message, 10, optional);
        }

        /// <summary>
        /// Display the message
        /// </summary>
        /// <param name="message">The message to display</param>
        public void WriteMessage(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Display an error
        /// </summary>
        /// <param name="error">The error to display</param>
        public void WriteError(string error)
        {
            Console.Error.WriteLine(error);
        }
    }
}
