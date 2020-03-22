using System;

namespace ConsoleFramework.Utils
{
    public static class Vector
    {
        /// <summary>
        /// Angle between two 2d vectors.
        /// https://onlinemschool.com/math/library/vector/angl
        /// </summary>
        /// <param name="ax">X position of vector a.</param>
        /// <param name="ay">Y position of vector a.</param>
        /// <param name="bx">X position of vector b.</param>
        /// <param name="by">Y position of vector b.</param>
        /// <returns>Angle between vectors in radians.</returns>
        public static double Angle(double ax, double ay, double bx, double by)
        {
            var dotproduct = ax * bx + ay * by;
            var amagnitude = Magnitude(ax, ay);
            var bmagnitude = Magnitude(bx, by);
            var anglecos = dotproduct / (amagnitude * bmagnitude);
            var result = Math.Acos(anglecos);
            return result;
        }

        /// <summary>
        /// Vector magnitude or length.
        /// </summary>
        /// <param name="x">X position of vector.</param>
        /// <param name="y">Y position of vector.</param>
        /// <returns>Vector magnitude.</returns>
        public static double Magnitude(double x, double y)
        {
            return Math.Sqrt(x * x + y * y);
        }
    }
}
