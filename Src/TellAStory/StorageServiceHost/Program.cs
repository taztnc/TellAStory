using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using StorageService;
using FacebookAPIService;

namespace Hosts
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost storageServiceHost = null;
            ServiceHost facebookAPIServiceHost = null;
            try
            {
                storageServiceHost = StartStorageService();
                facebookAPIServiceHost = StartFacebookAPIService();

                Console.WriteLine();
                Console.WriteLine("Press 'F1' to exit");
                while (Console.ReadKey().Key != ConsoleKey.F1) ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception: {0}", ex.Message));
            }

            if (storageServiceHost != null)
            {
                storageServiceHost.Close();
            }

            if (facebookAPIServiceHost != null)
            {
                facebookAPIServiceHost.Close();
            }
       }

        private static ServiceHost StartFacebookAPIService()
        {
            var facebookAPIServiceHost = new ServiceHost(typeof(FacebookAPIService.FacebookAPIService));
            facebookAPIServiceHost.Open();
            Console.WriteLine("Facebook API Service started");
            return facebookAPIServiceHost;
        }

        private static ServiceHost StartStorageService()
        {
            var storageServiceHost = new ServiceHost(typeof(StupidStorageService));
            storageServiceHost.Open();
            Console.WriteLine("Storage Service started");
            return storageServiceHost;
        }
    }
}
