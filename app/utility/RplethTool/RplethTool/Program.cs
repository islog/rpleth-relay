using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RplethTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Interface inter = new Interface();
            Options options = new Options();
            try
            {
                options.Parse(args);
                inter.Welcome();
                if (options.Menu == -1)
                    options.Menu = inter.GetMenu();
                if (options.Menu != inter.confList.ConfigurationsList.Count)
                {
                    Console.WriteLine(inter.confList.ConfigurationsList[options.Menu]);
                    inter.confList.ConfigurationsList[options.Menu].GetConfiguration(options, inter);
                    inter.confList.ConfigurationsList[options.Menu].Write();
                }
                inter.GoodBy();
                Thread.Sleep(5000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                inter.GoodBy();
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }
    }
}
