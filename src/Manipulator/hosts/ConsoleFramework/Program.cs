namespace ConsoleFramework
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using ConsoleFramework.Manipulator;
    using HidLibrary;
    using ConsoleFramework.Utils;
    using ConsoleFramework.Native;

    public class Program
    {
        static async Task Main(string[] args)
        {
            //var quadInit2 = Quadrilateral.WithTwoSidesTwoAngles(98, 96, 128.57, 100.8);
            //var quadDelta2 = Quadrilateral.WithFourSides(98, 96, quadInit2.SideC, Math.Abs(quadInit2.SideD - 20));
            //var quadInit2 = Quadrilateral.WithTwoSidesTwoAngles(98, 96, 141.17, 101.25);
            //var quadDelta2 = Quadrilateral.WithFourSides(98, 96, quadInit2.SideC, quadInit2.SideD + 30);

            // TODO: move to tests
            //var quad1 = Quadrilateral.WithTwoSidesTwoAngles(7.21, 12.17, 123.69, 65.77); // pass
            //var quad2 = Quadrilateral.WithFourSides(7.21, 12.17, 8, 8); // pass
            //var quad1 = Quadrilateral.WithTwoSidesTwoAngles(6.32, 6.32, 71.57, 126.87); // pass
            //var quad2 = Quadrilateral.WithFourSides(6.32, 6.32, 8, 8); // pass
            //var quad1 = Quadrilateral.WithTwoSidesTwoAngles(10.2, 6.32, 78.69, 82.87); // pass
            //var quad2 = Quadrilateral.WithFourSides(10.2, 6.32, 8, 8); // pass
            //var quad1 = Quadrilateral.WithTwoSidesTwoAngles(10.77, 12.17, 111.8, 58.74); // pass
            //var quad2 = Quadrilateral.WithFourSides(10.77, 12.17, 8, 8); // pass

            var device = new Controller();

            await Program.PointUp(device);

            var positions = await device.ServoPositionRead();
            Console.WriteLine(string.Join(",", positions));

            // Known:
            // side from base to first joint
            // a = 98 mm
            // side from first join to arm
            // b = 96 mm
            // angle between b and a (from side b to a clockwise)
            // ba = positions[3]: servo4[100, 500, 850] => ba(90, 180, 270)
            // angle between a and d (from side a to d clockwise)
            // ad = positions[4]: servo5[100, 500, 850] => ad(0, 90, 180)
            // angle between d and c (from side a to d clockwise) always 90 degrees
            // dc = 90 degrees
            // base direction angle
            // z = positions[0]: servo6[-90(right), 0, 90(left)] => [120, 500, 880]
            // direction vector in mm
            // v = (+/-x, +/-y, +/-z)

            //await device.MultiServoMove(1000, servo4: 500);
            //await device.MultiServoMove(1000, 700, 700, 850, 150, 600, 500);

            //positions = await device.ServoPositionRead();
            //Console.WriteLine(string.Join(",", positions));

            var xchange = 30;
            var ychange = 10;
            var zchange = 10;

            var ba = Arm.S4ToAngle(positions[3]);
            var ad = Arm.S5ToAngle(positions[4]);
            var z0 = Arm.S6ToAngle(positions[5]);

            // coordinates of ba point
            var ad_rad = Converter.DegreesToRadians(ad);
            var z0_rad = Converter.DegreesToRadians(z0);
            var ba_z = Arm.A * Math.Cos(ad_rad) * Math.Sin(z0_rad);
            var ba_x = Arm.A * Math.Cos(ad_rad) * Math.Cos(z0_rad);
            var ba_y = Arm.A * Math.Sin(ad_rad);

            // caclulating coordinates of cb point
            var o = Triangle.Opposite(Arm.A, Arm.B, z0 > 180.0 ? z0 - 180.0 : z0);
            var o_delta = Triangle.Angle(Arm.A, o, Arm.B);
            var od = ad + (z0 < 180.0 ? -1 : 1) * o_delta;

            // coordinates of cb point
            var od_rad = Converter.DegreesToRadians(od);
            var cb_z = o * Math.Cos(od_rad) * Math.Sin(z0_rad);
            var cb_x = o * Math.Cos(od_rad) * Math.Cos(z0_rad);
            var cb_y = o * Math.Sin(od_rad);

            // coordinates of cb1 point
            var cb1_z = cb_z + xchange;
            var cb1_x = cb_x + ychange;
            var cb1_y = cb_y + zchange;

            // calculate new position of ba1
            var o_magnitude = Math.Sqrt(cb1_x * cb1_x + cb1_y * cb1_y + cb1_z * cb1_z);
            var ba1 = Triangle.Angle(Arm.A, Arm.B, o_magnitude);

            // calculate new position of ad1
            var ad1_upper = Triangle.Angle(Arm.A, o_magnitude, Arm.B);
            var ad1_bottom = Triangle.Right.Angle(o_magnitude, cb1_y);
            var ad1 = ad1_upper + ad1_bottom;

            // calculate new angles z1
            var z1 = Converter.RadiansToDegrees(Math.Acos(cb1_x / (o_magnitude * Converter.RadiansToDegrees(Math.Cos(ad1)))));

            var quadInit = Quadrilateral.WithTwoSidesTwoAngles(Arm.A, Arm.B, ad, ba);

            var x0 = Triangle.Right.Base(quadInit.SideD, z0);
            var y0 = Triangle.Right.Opposite(quadInit.SideD, z0);

            var x1 = x0 + xchange;
            var y1 = y0 + zchange;

            var zDelta = Vector.Angle(x0, y0, x1, y1);
            var z1 = z0 + zDelta;

            var quadDelta =
                Quadrilateral.WithFourSides(
                    Arm.A,
                    Arm.B,
                    Math.Abs(quadInit.SideC + ychange),
                    Math.Abs(quadInit.SideD + xchange)
                );

            Console.WriteLine(quadDelta.AngleAD);
            Console.WriteLine(quadDelta.AngleBA);

            await device.MultiServoMove(
                1000,
                servo4: Arm.AngleToS4(quadDelta.AngleBA),
                servo5: Arm.AngleToS5(quadDelta.AngleAD),
                servo6: Arm.AngleToS6(z1)
            );
            
            //for (int i = 0; i < 10; i++)
            //{
            //    await device.MultiServoMove(250, 700, 700, 613, 179, 738, 512);
            //    await device.MultiServoMove(250, 700, 700, 570, 203, 799, 456);
            //    await device.MultiServoMove(250, 700, 700, 542, 279, 896, 456);
            //    await device.MultiServoMove(250, 700, 700, 542, 376, 981, 478);
            //    await device.MultiServoMove(250, 700, 700, 521, 390, 995, 521);
            //    await device.MultiServoMove(250, 700, 700, 522, 356, 972, 565);
            //    await device.MultiServoMove(250, 700, 700, 538, 300, 916, 591);
            //    await device.MultiServoMove(250, 700, 700, 565, 171, 761, 591);
            //}

            //await Program.PointUp(device);

            //Console.WriteLine(string.Join(",", await device.ServoPositionRead()));

            Console.ReadLine();
        }

        private static Task PointUp(Controller device)
        {
            // [front(+)-back(-)],[right(-)-left(+)]
            return device.MultiServoMove(
                1000,
                servo1: 700, // [140(Open), 700(Closed)]
                servo2: 680, // [0, 1000]
                servo3: 510, // [90 - 130, 180 - 510, 270 - 880]
                servo4: 500, // [90 - 130, 180 - 500, 270 - 870]
                servo5: 490, // [0 - 120, 90 - 490, 180 - 850]
                servo6: 500); // [-90(right) - 120, 0 - 500, 90(left) - 880]
        }
    }
}