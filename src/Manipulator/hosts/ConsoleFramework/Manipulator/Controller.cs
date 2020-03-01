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

        private readonly HidDevice device;

        private readonly object disposeLockGate = new object();

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
                bw.Write((byte)0); // id
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

        public async Task<ushort[]> ServoPositionRead()
        {
            var buffer = new byte[device.Capabilities.OutputReportByteLength];

            using (MemoryStream ms = new MemoryStream(buffer))
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write((byte)0);
                bw.Write((ushort)0x5555);

                bw.Write((byte)9); // Length
                bw.Write((byte)RobotCommand.ServoPositionRead); /* 1 */ // Command: ServoPositionRead = 21

                bw.Write((byte)6); /* 1 */ // (byte)count
                bw.Write((byte)1); /* 1 */ // (byte)servo
                bw.Write((byte)2); /* 1 */ // (byte)servo
                bw.Write((byte)3); /* 1 */ // (byte)servo
                bw.Write((byte)4); /* 1 */ // (byte)servo
                bw.Write((byte)5); /* 1 */ // (byte)servo
                bw.Write((byte)6); /* 1 */ // (byte)servo
            }

            await device.WriteAsync(buffer, 300);

            //Console.WriteLine($"{buffer.Length} bytes transmitted.");

            var report = device.ReadReport();

            //Console.WriteLine($"{report.Data.Length} bytes received.");

            var length = report.Data[3] + 3;
            var data = new byte[length];
            Array.Copy(report.Data, data, length);

            //Console.WriteLine(BitConverter.ToString(data));

            // (byte)count { (byte)servo (ushort)position }
            //  0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19 20 21 22 23
            // 55-55-15-15-06-01-CD-02-02-B2-02-03-39-00-04-9B-03-05-8B-03-06-80-00-42
            // 55-55-15-15-06-01-CD-02-02-B2-02-03-39-00-04-9B-03-05-8A-03-06-81-00-20
            // 55-55-12-15-05-01-CD-02-02-B2-02-03-39-00-04-9B-03-05-8A-03-00-10-00-00
            //       len   cnt 1        2        3        4        5        6
            //          cmd

            var servo1 = BitConverter.ToUInt16(data, 6);
            var servo2 = BitConverter.ToUInt16(data, 9);
            var servo3 = BitConverter.ToUInt16(data, 12);
            var servo4 = BitConverter.ToUInt16(data, 15);
            var servo5 = BitConverter.ToUInt16(data, 18);
            var servo6 = BitConverter.ToUInt16(data, 21);

            ushort[] arr = { servo1, servo2, servo3, servo4, servo5, servo6 };
            return arr;
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
            if (device == null || !device.IsOpen)
            {
                return;
            }

            lock (this.disposeLockGate)
            {
                if (device == null || !device.IsOpen)
                {
                    return;
                }

                device.CloseDevice();
            }
        }
    }
}
