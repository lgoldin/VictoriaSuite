using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victoria.Shared
{
    public static class NodeRandomValue
    {
        private static string value = null;

        public static string getValue(CultureInfo cultureInfo = null)
        {
            if (value == null)
                value = new Random().NextDouble().ToString("F6", cultureInfo);

                return value;
        }

        public static void setValue(string newValue)
        {
            value = newValue;
        }
    }
}
