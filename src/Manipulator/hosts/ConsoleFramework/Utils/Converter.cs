using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleFramework.Utils
{
    public static class Converter
    {
        /// <summary>
        /// Convert degrees to radians.
        /// </summary>
        /// <param name="degrees">Degrees.</param>
        /// <returns>Radians.</returns>
        public static double DegreesToRadians(double degrees)
        {
            var radians = (Math.PI / 180) * degrees;
            return radians;
        }

        /// <summary>
        /// Convert radians to degrees.
        /// </summary>
        /// <param name="radians">Radians.</param>
        /// <returns>Degrees.</returns>
        public static double RadiansToDegrees(double radians)
        {
            double degrees = (180 / Math.PI) * radians;
            return degrees;
        }
    }
}
