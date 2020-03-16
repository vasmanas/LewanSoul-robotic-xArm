namespace ConsoleFramework
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using ConsoleFramework.Manipulator;
    using HidLibrary;
    using ConsoleFramework.Utils;

    public class Program
    {
        static async Task Main(string[] args)
        {
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

            var positions = await device.ServoPositionRead();
            Console.WriteLine(string.Join(",", positions));

            //await Program.PointUp(device);

            //await device.MultiServoMove(1000, servo4: 400);
            //await device.MultiServoMove(1000, servo4: 600);
            //await device.MultiServoMove(1000, servo4: 500);

            await device.MultiServoMove(1000, 700, 700, 350, 200, 600, 500);

            positions = await device.ServoPositionRead();
            Console.WriteLine(string.Join(",", positions));

            // a = 98 mm
            // b = 96 mm
            // positions[4]: servo5[100, 500, 850] => ad(0, 90, 180)
            // positions[3]: servo4[100, 500, 850] => ba(90, 180, 270)

            // positions[3]: servo4[100, 500, 850] => ba(90, 180, 270)
            Func<double, double> calcba = pos =>
                (pos <= 500 ? (90 * (pos - 100)) / 400 : ((90 * (pos - 500)) / 350) + 90) + 90;

            // positions[4]: servo5[100, 500, 850] => ad(0, 90, 180)
            Func<double, double> calcad = pos =>
                pos <= 500 ? (90 * (pos - 100)) / 400 : ((90 * (pos - 500)) / 350) + 90;

            var ba = calcba(positions[3]);
            var ad = calcad(positions[4]);

            var quadInit = Quadrilateral.WithTwoSidesTwoAngles(98, 96, ad, ba);
            var quadDelta = Quadrilateral.WithFourSides(98, 96, quadInit.SideC - 50, quadInit.SideD); // pass

            Console.WriteLine(quadDelta.AngleAD);
            Console.WriteLine(quadDelta.AngleBA);

            // ba(90, 180, 270) => servo4[100, 500, 850]
            Func<double, double> calcServo4 = angle =>
                angle < 180 ? (400 * (angle - 90) / 90) + 100 : (350 * (angle - 180) / 90) + 500;

            // ad(0, 90, 180) => servo5[100, 500, 850]
            Func<double, double> calcServo5 = angle =>
                angle < 90 ? (400 * angle / 90) + 100 : (350 * (angle - 90) / 90) + 500;

            await device.MultiServoMove(
                1000,
                servo4: (ushort)calcServo4(quadDelta.AngleBA),
                servo5: (ushort)calcServo5(quadDelta.AngleAD)
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
                servo3: 500, // [90 - 150, 180 - 500, 270 - 900]
                servo4: 500, // [90 - 100, 180 - 500, 270 - 850]
                servo5: 500, // [0 - 100, 90 - 500, 180 - 850]
                servo6: 500); // [0, 1000]
        }
    }
}