using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Citacka_hesiel
{
    class Program
    {

        static void Main(string[] args)
        {
            ProcessPasswordFile passFileProcessor = new ProcessPasswordFile("hesla", true, true);
            passFileProcessor.processFile();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
