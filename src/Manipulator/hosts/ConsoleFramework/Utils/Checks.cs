using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleFramework.Utils
{
    public static class Checks
    {
        public static void GreaterThanZero(double value, string parameterName)
        {
            if (value <= 0)
            {
                throw new Exception($"Parameter {parameterName} must be greater than zero");
            }
        }

        public static void Between(double value, double from, double to, string parameterName)
        {
            if (from > to)
            {
                throw new Exception("From must be lower or equal than to");
            }

            if (from <= value && value <= to)
            {
                throw new Exception($"Parameter {parameterName} must be between {from} and {to} degrees");
            }
        }
    }
}
