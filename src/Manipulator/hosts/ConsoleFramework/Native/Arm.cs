namespace ConsoleFramework.Native
{
    public static class Arm
    {
        public const double A = 98; // mm
        public const double B = 96; // mm

        private static double ToAngle(ushort a90, ushort a180, ushort a270, ushort value)
        {
            var dif = value < a180 ? a180 - a90 : a270 - a180;
            var bas = value < a180 ? a90 : a180;
            var koef = value < a180 ? 1.0 : 2.0;

            return 90.0 * ((koef * dif + value - bas) / dif);
        }

        private static ushort ToServo(ushort a90, ushort a180, ushort a270, double angle)
        {
            var positiveAngle = angle < 0 ? 360 + angle : angle;

            var dif = angle < 180.0 ? a180 - a90 : a270 - a180;
            var bas = angle < 180.0 ? a90 : a180;
            var koef = angle < 180.0 ? 1.0 : 2.0;

            var value = positiveAngle * dif / 90.0 - koef * dif + bas;

            return (ushort)value;
        }

        /// <summary>
        /// Angle to servo 3.
        /// </summary>
        /// <param name="angle">Angle in degrees between 90 and 270.</param>
        /// <returns>Servo value.</returns>
        public static ushort AngleToS3(double angle)
        {
            return ToServo(130, 510, 880, angle);
        }

        /// <summary>
        /// Angle to servo 4.
        /// </summary>
        /// <param name="angle">Angle in degrees between 90 and 270.</param>
        /// <returns>Servo value.</returns>
        public static ushort AngleToS4(double angle)
        {
            return ToServo(130, 500, 870, angle);
        }

        /// <summary>
        /// Angle to servo 5.
        /// </summary>
        /// <param name="angle">Angle in degrees between 90 and 270.</param>
        /// <returns>Servo value.</returns>
        public static ushort AngleToS5(double angle)
        {
            return ToServo(120, 490, 850, angle + 90.0);
        }

        /// <summary>
        /// Angle to servo 6.
        /// </summary>
        /// <param name="value">Angle in degrees between -90 and 90. </param>
        /// <returns>Servo value.</returns>
        public static ushort AngleToS6(double angle)
        {
            const ushort A_90 = 120;
            const ushort A0 = 500;
            const ushort A90 = 880;

            if (angle == 0.0)
            {
                return 0;
            }

            var dif = angle < 0.0 ? (A0 - A_90) : (A90 - A0);
            var value = angle * dif / 90.0 + A0;
            return (ushort)value;
        }

        /// <summary>
        /// Servo 3 to angle.
        /// </summary>
        /// <param name="value">Servo value.</param>
        /// <returns>Angle in degrees between 90 and 270. </returns>
        public static double S3ToAngle(ushort value)
        {
            return ToAngle(130, 510, 880, value);
        }


        /// <summary>
        /// Servo 4 to angle.
        /// </summary>
        /// <param name="value">Servo value.</param>
        /// <returns>Angle in degrees between 90 and 270. </returns>
        public static double S4ToAngle(ushort value)
        {
            return ToAngle(130, 500, 870, value);
        }

        /// <summary>
        /// Servo 5 to angle.
        /// </summary>
        /// <param name="value">Servo value.</param>
        /// <returns>Angle in degrees between 90 and 270. </returns>
        public static double S5ToAngle(ushort value)
        {
            return ToAngle(120, 490, 850, value) - 90.0;
        }

        /// <summary>
        /// Servo 6 to angle.
        /// </summary>
        /// <param name="value">Servo value.</param>
        /// <returns>Angle in degrees between -90 and 90. </returns>
        public static double S6ToAngle(ushort value)
        {
            const ushort A_90 = 120;
            const ushort A0 = 500;
            const ushort A90 = 880;

            if (value == A0)
            {
                return 0.0;
            }

            var dif = value < A0 ? (A0 - A_90) : (A90 - A0);
            return 90.0 * (value - A0) / dif;
        }
    }
}
