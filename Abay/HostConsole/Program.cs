using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ServiceLibrary;

namespace HostConsole
{
    class Program
    {
        static void Main()
        {
            ServiceHost SUserLogin = new ServiceHost(typeof(UserLogin));
            SUserLogin.Open();
            Console.WriteLine("UserLogin service started.");
            ServiceHost SItemService = new ServiceHost(typeof(ItemService));
            SItemService.Open();
            Console.WriteLine("Item service started.");

            Console.WriteLine("Press any key to close the services");
            Console.ReadLine();

            SUserLogin.Close();
            SItemService.Close();
        }
    }
}
