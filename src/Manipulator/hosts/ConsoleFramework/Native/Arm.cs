namespace ConsoleFramework.Native
{
    public static class Arm
    {
        private static double ToAngle(ushort a90, ushort a180, ushort a270, ushort value)
        {
            var dif = value < a180 ? a180 - a90 : a270 - a180;
            var bas = value < a180 ? a90 : a180;
            var koef = value < a180 ? 1.0 : 2.0;

            return 90.0 * ((koef * dif + value - bas) / dif);
        }

        public static double S3toAngle(ushort value)
        {
            return ToAngle(130, 510, 880, value);
        }

        public static double S4toAngle(ushort value)
        {
            return ToAngle(130, 500, 870, value);
        }

        public static double S5toAngle(ushort value)
        {
            return ToAngle(120, 490, 850, value);
        }

        public static double s6toAngle(ushort value)
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
