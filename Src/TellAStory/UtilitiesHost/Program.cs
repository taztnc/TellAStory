using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogBookService;
using System.ServiceModel;

namespace UtilitiesHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost logBookServiceHost = null;
            try
            {
                logBookServiceHost = StartLogBookServiceHost();
                Console.WriteLine();
                Console.WriteLine("Press 'F1' to exit");
                while (Console.ReadKey().Key != ConsoleKey.F1) ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception: {0}", ex.Message));
            }

            if (logBookServiceHost != null)
            {
                logBookServiceHost.Close();
            }
       }

        private static ServiceHost StartLogBookServiceHost()
        {
            var logBookServiceHost = new ServiceHost(typeof(LogBookService.LogBookService));
            logBookServiceHost.Open();
            Console.WriteLine("LogBook Service started");
            return logBookServiceHost;
        }
    }
}
