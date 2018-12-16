namespace Controller
{
    public class ValidateInput
    {
        public ValidateInput() { }

        public bool CheckInt(int value)
        {
            return value < 0 ? false : true;
        }

        public bool CheckDouble(double value)
        {
            return value < 0 ? false : true;
        }

        public bool CheckString(string value, int length)
        {
            return value.Length < length ? false : true;
        }
    }
}
