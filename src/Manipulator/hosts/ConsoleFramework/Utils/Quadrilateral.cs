using System;

namespace ConsoleFramework.Utils
{
    // https://keisan.casio.com/exec/system/1273849674
    // https://www.math10.com/en/geometry/geogebra/geogebra.html
    // http://latex.codecogs.com/eqneditor/editor.php

    public class Quadrilateral
    {
        /// <summary>
        /// Create Quadrilateral.
        /// </summary>
        private Quadrilateral() {}

        public double SideA { get; private set; }
        public double SideB { get; private set; }
        public double SideC { get; private set; }
        public double SideD { get; private set; }

        // Alpha
        public double AngleAD { get; private set; }

        // Beta
        public double AngleCB { get; private set; }

        // Gamma
        public double AngleBA { get; private set; }

        /// <summary>
        /// Create Quadrilateral.
        /// </summary>
        /// <param name="a">Lenth of side a.</param>
        /// <param name="b">Lenth of side b.</param>
        /// <param name="ad">AD angle in degrees.</param>
        /// <param name="ba">BA angle in degrees.</param>
        /// <returns>New quadrilateral.</returns>
        public static Quadrilateral WithTwoSidesTwoAngles(double a, double b, double ad, double ba)
        {
            Checks.GreaterThanZero(a, nameof(a));
            Checks.GreaterThanZero(b, nameof(b));
            Checks.Between(ad, 0, 360, nameof(ad));
            Checks.Between(ba, 0, 360, nameof(ba));

            var quad = new Quadrilateral();

            quad.SideA = a;
            quad.SideB = b;
            quad.AngleAD = ad;
            quad.AngleBA = ba;

            quad.AngleCB = 270 - quad.AngleAD - quad.AngleBA;
            var bx = Triangle.Right.Opposite(quad.SideB, quad.AngleCB);
            var by = Triangle.Right.Side(quad.SideB, bx);
            var ay = Triangle.Right.Opposite(quad.SideA, quad.AngleAD);
            quad.SideC = Quadrilateral.CalculateC(quad.AngleCB, ay, by);
            var ax = Triangle.Right.Side(quad.SideA, ay);
            quad.SideD = Quadrilateral.CalculateD(quad.AngleAD, ax, bx);

            return quad;
        }

        /// <summary>
        /// Create Quadrilateral.
        /// </summary>
        /// <param name="a">Lenth of side a.</param>
        /// <param name="b">Lenth of side b.</param>
        /// <param name="c">Lenth of side c.</param>
        /// <param name="d">Lenth of side d.</param>
        /// <returns>New quadrilateral.</returns>
        public static Quadrilateral WithFourSides(double a, double b, double c, double d)
        {
            Checks.GreaterThanZero(a, nameof(a));
            Checks.GreaterThanZero(b, nameof(b));
            Checks.GreaterThanZero(c, nameof(c));
            Checks.GreaterThanZero(d, nameof(d));

            var quad = new Quadrilateral();

            quad.SideA = a;
            quad.SideB = b;
            quad.SideC = c;
            quad.SideD = d;

            var e = Triangle.Right.Hypotenuse(c, d);
            var ad1 = Converter.RadiansToDegrees(Math.Asin(c / e));
            var cb1 = 90 - ad1;
            var ad2 = Triangle.Angle(quad.SideA, e, quad.SideB);
            var cb2 = Triangle.Angle(quad.SideB, e, quad.SideA);
            quad.AngleAD = ad1 + ad2;
            quad.AngleCB = cb1 + cb2;
            quad.AngleBA = 270 - quad.AngleAD - quad.AngleCB;

            return quad;
        }

        /// <summary>
        /// Calculate c side.
        /// </summary>
        /// <param name="cb">CB angle in degrees.</param>
        /// <param name="ay">Lenth of side a_y.</param>
        /// <param name="by">Lenth of side b_y.</param>
        /// <returns></returns>
        private static double CalculateC(double cb, double ay, double by)
        {
            Checks.Between(cb, -90, 180, nameof(cb));
            Checks.GreaterThanZero(ay, nameof(ay));
            Checks.GreaterThanZero(by, nameof(by));

            if (cb < 90)
            {
                var c = ay + by;
                return Math.Abs(c);
            }
            else if (cb == 90)
            {
                var c = ay;
                return Math.Abs(c);
            }
            else // beta > 90
            {
                var c = ay - by;
                return Math.Abs(c);
            }
        }

        /// <summary>
        /// Calculate d side.
        /// </summary>
        /// <param name="ad">AD angle in degrees.</param>
        /// <param name="ax">Lenth of side a_x.</param>
        /// <param name="bx">Lenth of side b_x.</param>
        /// <returns></returns>
        private static double CalculateD(double ad, double ax, double bx)
        {
            Checks.Between(ad, -90, 180, nameof(ad));
            Checks.GreaterThanZero(ax, nameof(ax));
            Checks.GreaterThanZero(bx, nameof(bx));

            if (ad < 90)
            {
                var d = bx + ax;
                return Math.Abs(d);
            }
            else if (ad == 90)
            {
                var d = bx;
                return Math.Abs(d);
            }
            else // alpha > 90
            {
                var d = bx - ax;
                return Math.Abs(d);
            }
        }
    }
}
