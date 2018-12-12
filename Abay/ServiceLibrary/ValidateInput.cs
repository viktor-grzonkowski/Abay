using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    class ValidateInput
    {
        public ValidateInput() { }

        public bool CheckInt(int value)
        {
            return value <= 0 ? false : true;
        }

        public bool CheckDouble(double value)
        {
            return value <= 0 ? false : true;
        }

        public bool CheckString(string value, int lenght)
        {
            return value.Length <= lenght ? false : true;
        }
    }
}
