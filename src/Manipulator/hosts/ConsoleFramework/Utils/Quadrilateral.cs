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
        /// <param name="alpha">Alpha angle in degrees.</param>
        /// <param name="gamma">Gamma angle in degrees.</param>
        /// <returns>New quadrilateral.</returns>
        public static Quadrilateral WithTwoSidesTwoAngles(double a, double b, double alpha, double gamma)
        {
            Checks.GreaterThanZero(a, nameof(a));
            Checks.GreaterThanZero(b, nameof(b));
            Checks.Between(alpha, -90, 180, nameof(alpha));
            Checks.Between(gamma, -90, 180, nameof(gamma));

            var quad = new Quadrilateral();

            quad.SideA = a;
            quad.SideB = b;
            quad.AngleAD = alpha;
            quad.AngleBA = gamma;

            quad.AngleCB = 270 - quad.AngleAD - quad.AngleBA;
            var bx = Triangle.Opposite(quad.SideB, quad.AngleCB);
            var by = Triangle.Side(quad.SideB, bx);
            var ay = Triangle.Opposite(quad.SideA, quad.AngleAD);
            quad.SideC = Quadrilateral.CalculateC(quad.AngleCB, ay, by);
            var ax = Triangle.Side(quad.SideA, ay);
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

            var e = Triangle.Hypotenuse(c, d);
            var alpha1 = Math.Asin(c / e);
            var beta1 = 90 - alpha1;
            var alpha2 = Triangle.Angle(quad.SideA, e, quad.SideB);
            var beta2 = Triangle.Angle(quad.SideB, e, quad.SideA);
            quad.AngleAD = alpha1 + alpha2;
            quad.AngleCB = beta1 + beta2;
            quad.AngleBA = 270 - quad.AngleAD - quad.AngleCB;

            return quad;
        }

        /// <summary>
        /// Calculate c side.
        /// </summary>
        /// <param name="beta">Beta angle in degrees.</param>
        /// <param name="ay">Lenth of side a_y.</param>
        /// <param name="by">Lenth of side b_y.</param>
        /// <returns></returns>
        private static double CalculateC(double beta, double ay, double by)
        {
            Checks.Between(beta, -90, 180, nameof(beta));
            Checks.GreaterThanZero(ay, nameof(ay));
            Checks.GreaterThanZero(by, nameof(by));

            if (beta < 90)
            {
                var c = ay + by;
                return c;
            }
            else if (beta == 90)
            {
                var c = ay;
                return c;
            }
            else // beta > 90
            {
                var c = ay - by;
                return c;
            }
        }

        /// <summary>
        /// Calculate d side.
        /// </summary>
        /// <param name="alpha">Alpha angle in degrees.</param>
        /// <param name="ax">Lenth of side a_x.</param>
        /// <param name="bx">Lenth of side b_x.</param>
        /// <returns></returns>
        private static double CalculateD(double alpha, double ax, double bx)
        {
            Checks.Between(alpha, -90, 180, nameof(alpha));
            Checks.GreaterThanZero(ax, nameof(ax));
            Checks.GreaterThanZero(bx, nameof(bx));

            if (alpha < 90)
            {
                var d = bx + ax;
                return d;
            }
            else if (alpha == 90)
            {
                var d = bx;
                return d;
            }
            else // alpha > 90
            {
                var d = bx - ax;
                return d;
            }
        }
    }
}
