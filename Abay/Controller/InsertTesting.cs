using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace Controller
{
    public class InsertTesting
    {
        TestInsert testing = new TestInsert();

        public InsertTesting()
        {
            
        }

        public void InsertData()
        {
            testing.Insert();
        }
    }
}
