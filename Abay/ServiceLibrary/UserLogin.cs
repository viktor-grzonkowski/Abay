using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Controller;
using ServiceLibrary.ServiceInterfaces;

namespace ServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserLogin" in both code and config file together.
    public class UserLogin : IUserLogin
    {
        InsertTesting insertDatShit = new InsertTesting();

        public void TestInsert()
        {
            insertDatShit.InsertData();
        }
    }
}
