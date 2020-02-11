using HidLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleFramework.Manipulator
{
    public class Controller : IDisposable
    {
        private const int vid = 0x0483;
        private const int pid = 0x5750;

        private HidDevice device;

        public HidDevice Device
        {
            get {
                return device;
            }
        }

        public Controller()
        {
            device = HidDevices.Enumerate(vid, pid).FirstOrDefault();
            if (device == null)
            {
                Console.WriteLine("xArm not found.");
                Console.ReadLine();

                return;
            }

            Console.WriteLine("xArm connected.");

            // TODO: Is this needed?
            Thread.Sleep(1000);

            device.OpenDevice();
            device.Inserted += Device_Inserted;
            device.Removed += Device_Removed;
            device.MonitorDeviceEvents = true;
        }

        public async Task MultiServoMove(
            ushort milliseconds,
            ushort? servo1 = null,
            ushort? servo2 = null,
            ushort? servo3 = null,
            ushort? servo4 = null,
            ushort? servo5 = null,
            ushort? servo6 = null)
        {
            var buffer = new byte[device.Capabilities.OutputReportByteLength];

            using (MemoryStream ms = new MemoryStream(buffer))
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write((byte)0);
                bw.Write((ushort)0x5555);

                var servoCount =
                    (servo1 == null ? 0 : 1)
                    + (servo2 == null ? 0 : 1)
                    + (servo3 == null ? 0 : 1)
                    + (servo4 == null ? 0 : 1)
                    + (servo5 == null ? 0 : 1)
                    + (servo6 == null ? 0 : 1);

                var length = 1 + 1 + 1 + 2 + servoCount * 3;

                bw.Write((byte)length); // Length
                bw.Write((byte)RobotCommand.MultiServoMove); // Command: MultiServoMove = 3

                bw.Write((byte)servoCount); // (byte)count
                bw.Write((ushort)milliseconds); // (ushort)milliseconds

                if (servo1 != null)
                {
                    bw.Write((byte)1); // (byte)servo
                    bw.Write((ushort)servo1); // (ushort)position[FF00 = null]
                }
                if (servo2 != null)
                {
                    bw.Write((byte)2); // (byte)servo
                    bw.Write((ushort)servo2); // (ushort)position[FF00 = null]
                }
                if (servo3 != null)
                {
                    bw.Write((byte)3); // (byte)servo
                    bw.Write((ushort)servo3); // (ushort)position[FF00 = null]
                }
                if (servo4 != null)
                {
                    bw.Write((byte)4); // (byte)servo
                    bw.Write((ushort)servo4); // (ushort)position[FF00 = null]
                }
                if (servo5 != null)
                {
                    bw.Write((byte)5); // (byte)servo
                    bw.Write((ushort)servo5); // (ushort)position[FF00 = null]
                }
                if (servo6 != null)
                {
                    bw.Write((byte)6); // (byte)servo
                    bw.Write((ushort)servo6); // (ushort)position[FF00 = null]
                }
            }

            var ss = await device.WriteAsync(buffer, 300);
            Console.WriteLine(ss ? "Report sent." : "Read timeout.");

            await Task.Delay(milliseconds + 10);
        }

        private void Device_Removed()
        {
            Console.WriteLine("xArm removed.");
        }

        private void Device_Inserted()
        {
            Console.WriteLine("xArm inserted.");
        }

        public void Dispose()
        {
            if (device != null && device.IsOpen)
            {
                device.CloseDevice();
            }
        }
    }
}
