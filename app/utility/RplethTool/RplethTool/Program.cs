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
                    while (!inter.confList.ConfigurationsList[options.Menu].GetConfiguration(options, inter))
                    {
                        inter.WriteMessage("You made a mistake start again.");
                    }
                    inter.confList.ConfigurationsList[options.Menu].Work(inter, options);
                }
                inter.GoodBye();
            }
            catch (Exception e)
            {
                inter.WriteError(e.Message);
                inter.GoodBye();
            }
        }
    }
}
