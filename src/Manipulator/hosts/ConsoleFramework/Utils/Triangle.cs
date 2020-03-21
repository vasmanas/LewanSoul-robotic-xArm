using System;

namespace ConsoleFramework.Utils
{
    public static class Triangle
    {
        public static class Right
        {
            /// <summary>
            /// Calculate opposite from hypotenuse and angle.
            /// https://keisan.casio.com/exec/system/1273849674.
            /// </summary>
            /// <param name="hypotenuse">Hypotenuse length.</param>
            /// <param name="angle">Angle in degrees.</param>
            /// <returns>Opposite length.</returns>
            public static double Opposite(double hypotenuse, double angle)
            {
                Checks.GreaterThanZero(hypotenuse, nameof(hypotenuse));
                Checks.Between(angle, -360, 360, nameof(angle));

                var opposite = hypotenuse * Math.Sin(Converter.DegreesToRadians(angle));
                return opposite;
            }

            /// <summary>
            /// Calculate opposite from hypotenuse and angle.
            /// https://keisan.casio.com/exec/system/1273849674.
            /// </summary>
            /// <param name="hypotenuse">Hypotenuse length.</param>
            /// <param name="angle">Angle in degrees.</param>
            /// <returns>Opposite length.</returns>
            public static double Base(double hypotenuse, double angle)
            {
                Checks.GreaterThanZero(hypotenuse, nameof(hypotenuse));
                Checks.Between(angle, -360, 360, nameof(angle));

                var baseSide = hypotenuse * Math.Cos(Converter.DegreesToRadians(angle));
                return baseSide;
            }

            /// <summary>
            /// Calculates side of right triangle from hypotenuse and side using Pythagorean theorem.
            /// https://en.wikipedia.org/wiki/Pythagorean_theorem.
            /// </summary>
            /// <param name="hypotenuse">Hypotenuse length.</param>
            /// <param name="side">Adjacent side length.</param>
            /// <returns>Another side length.</returns>
            public static double Side(double hypotenuse, double side)
            {
                Checks.GreaterThanZero(hypotenuse, nameof(hypotenuse));
                Checks.GreaterThanZero(side, nameof(side));

                var anotherSide = Math.Sqrt(hypotenuse * hypotenuse - side * side);
                return anotherSide;
            }

            /// <summary>
            /// Calculates hypotenuse of right triangle from sides using Pythagorean theorem.
            /// https://en.wikipedia.org/wiki/Pythagorean_theorem.
            /// </summary>
            /// <param name="sideA">Opposite side length.</param>
            /// <param name="sideB">Side length.</param>
            /// <returns>Hypotenuse length.</returns>
            public static double Hypotenuse(double sideA, double sideB)
            {
                Checks.GreaterThanZero(sideA, nameof(sideA));
                Checks.GreaterThanZero(sideB, nameof(sideB));

                var hypotenuse = Math.Sqrt(sideA * sideA + sideB * sideB);
                return hypotenuse;
            }
        }

        /// <summary>
        /// Calculates angle when all sides are known.
        /// </summary>
        /// <param name="sideA">Side length.</param>
        /// <param name="sideB">Side length.</param>
        /// <param name="opposite">Opposite side to an angle length.</param>
        /// <returns>Angle in degrees.</returns>
        public static double Angle(double sideA, double sideB, double opposite)
        {
            Checks.GreaterThanZero(sideA, nameof(sideA));
            Checks.GreaterThanZero(sideB, nameof(sideB));
            Checks.GreaterThanZero(opposite, nameof(opposite));

            var radians = Math.Acos((sideA * sideA + sideB * sideB - opposite * opposite) / (2 * sideA * sideB));
            var angle = Converter.RadiansToDegrees(radians);
            return angle;
        }
    }
}
