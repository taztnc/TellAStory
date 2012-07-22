using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using StorageService;

namespace Hosts
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var serviceHost = new ServiceHost(typeof(StupidStorageService));
                serviceHost.Open();
                Console.WriteLine("StupidStorageService");
                Console.ReadKey();
                serviceHost.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception: {0}", ex.Message));
                throw;
            }
        }
    }
}
