namespace ConsoleFramework
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using ConsoleFramework.Manipulator;
    using HidLibrary;

    public class Program
    {
        static async Task Main(string[] args)
        {
            var device = new Controller();

            //await device.MultiServoMove(1000, servo1: 0, servo2: 0, servo3: 500, servo6: 1000);
            //await device.MultiServoMove(1000, servo1: 500, servo2: 1000, servo3: 0, servo6: 0);
            //await device.MultiServoMove(1000, servo1: 200, servo2: 0, servo3: 700, servo6: 1000);
            //await device.MultiServoMove(1000, servo1: 700, servo2: 1000, servo3: 200, servo6: 0);
            //await device.MultiServoMove(1000, servo1: 300, servo2: 0, servo3: 1000, servo6: 1000);
            //await device.MultiServoMove(1000, servo1: 1000, servo2: 1000, servo3: 0, servo6: 0);

            var positions = await device.ServoPositionRead();
            Console.WriteLine(string.Join(",", positions));

            await Program.PointUp(device);

            for (int i = 0; i < 10; i++)
            {
                await device.MultiServoMove(250, 700, 700, 613, 179, 738, 512);
                await device.MultiServoMove(250, 700, 700, 570, 203, 799, 456);
                await device.MultiServoMove(250, 700, 700, 542, 279, 896, 456);
                await device.MultiServoMove(250, 700, 700, 542, 376, 981, 478);
                await device.MultiServoMove(250, 700, 700, 521, 390, 995, 521);
                await device.MultiServoMove(250, 700, 700, 522, 356, 972, 565);
                await device.MultiServoMove(250, 700, 700, 538, 300, 916, 591);
                await device.MultiServoMove(250, 700, 700, 565, 171, 761, 591);
            }

            await Program.PointUp(device);

            //await device.MultiServoMove(1000, servo2: 1000);
            //Console.WriteLine(string.Join(",", await device.ServoPositionRead()));

            Console.ReadLine();
        }

        private static Task PointUp(Controller device)
        {
            // [front(+)-back(-)],[right(-)-left(+)]
            return device.MultiServoMove(
                1000,
                servo1: 700, // [140(Open), 700(Closed)]
                servo2: 700, // [0, 1000]
                servo3: 400, // [0, 1000]
                servo4: 500, // [0, 1000]
                servo5: 850, // [0, 1000]
                servo6: 500); // [0, 1000]
        }
    }
}