using ServiceLibrary;
using System;
using System.ServiceModel;

namespace HostConsole
{
    class Program
    {
        static void Main()
        {
            ServiceHost sUserLogin = new ServiceHost(typeof(UserService));
            sUserLogin.Open();
            Console.WriteLine("UserLogin service started.");

            ServiceHost sItemService = new ServiceHost(typeof(ItemService));
            sItemService.Open();
            Console.WriteLine("Item service started.");

            ServiceHost sBidService = new ServiceHost(typeof(BidService));
            sBidService.Open();
            Console.WriteLine("Bid service started.");

            Console.WriteLine("Press any key to close the services");
            Console.ReadLine();

            sUserLogin.Close();
            sItemService.Close();
            sBidService.Close();
        }
    }
}
