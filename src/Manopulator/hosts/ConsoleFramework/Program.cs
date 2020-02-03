namespace ConsoleFramework
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using HidLibrary;

    public class Program
    {
        private const int vid = 0x0483;
        private const int pid = 0x5750;

        private static HidDevice device;

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            device = HidDevices.Enumerate(vid, pid).FirstOrDefault();
            if (device == null) {
                Console.WriteLine("xArm not found.");
                Console.ReadLine();

                return;
            }

            Console.WriteLine("xArm connected.");
            Thread.Sleep(1000);

            device.OpenDevice();
            device.Inserted += Device_Inserted;
            device.Removed += Device_Removed;
            device.MonitorDeviceEvents = true;

            byte[] buffer = new byte[device.Capabilities.OutputReportByteLength];

            using (MemoryStream ms = new MemoryStream(buffer))
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write((byte)0);
                bw.Write((ushort)0x5555);

                bw.Write((byte)10); // Length
                //bw.Write((byte)0x03); /* 1 */ // Command: MultiServoMove = 3
                bw.Write((byte)RobotCommand.MultiServoMove); /* 1 */ // Command: MultiServoMove = 3

                bw.Write((byte)0x02); /* 1 */ // (byte)count
                bw.Write((ushort)1000); /* 2 */ // (ushort)milliseconds

                bw.Write((byte)1); /* 1 */ // (byte)servo
                bw.Write((ushort)0x0000); /* 2 */ // (ushort)position[FF00 = null]

                bw.Write((byte)2); /* 1 */ // (byte)servo
                bw.Write((ushort)0x0000); /* 2 */ // (ushort)position[FF00 = null]
            }

            var ss = await device.WriteAsync(buffer, 300);
            Console.WriteLine(ss ? "Report sent." : "Read timeout.");
            //device.Write(buffer, (success) => { Console.WriteLine(success ? "Report sent." : "Read timeout."); }, 300);

            Thread.Sleep(1000);

            Console.WriteLine("Here 1");
            using (MemoryStream ms = new MemoryStream(buffer))
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write((byte)0);
                bw.Write((ushort)0x5555);

                bw.Write((byte)10); // Length
                //bw.Write((byte)0x03); /* 1 */ // Command: MultiServoMove = 3
                bw.Write((byte)RobotCommand.MultiServoMove); /* 1 */ // Command: MultiServoMove = 3

                bw.Write((byte)0x02); /* 1 */ // (byte)count
                bw.Write((ushort)2000); /* 2 */ // (ushort)milliseconds

                bw.Write((byte)1); /* 1 */ // (byte)servo
                bw.Write((ushort)0x0A99); /* 2 */ // (ushort)position[FF00 = null]

                bw.Write((byte)2); /* 1 */ // (byte)servo
                bw.Write((ushort)0x0A99); /* 2 */ // (ushort)position[FF00 = null]
            }
            Console.WriteLine("Here 2");

            device.Write(buffer, (success) => { Console.WriteLine(success ? "Report sent." : "Read timeout."); }, 300);

            Thread.Sleep(2000);

            Console.WriteLine("Here 3");
            using (MemoryStream ms = new MemoryStream(buffer))
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write((byte)0);
                bw.Write((ushort)0x5555);

                bw.Write((byte)6); // Length
                bw.Write((byte)RobotCommand.ServoPositionRead); /* 1 */ // Command: ServoPositionRead = 21

                bw.Write((byte)3); /* 1 */ // (byte)count
                bw.Write((byte)1); /* 1 */ // (byte)servo
                bw.Write((byte)2); /* 1 */ // (byte)servo
                bw.Write((byte)3); /* 1 */ // (byte)servo
            }
            Console.WriteLine("Here 4");

            device.Write(buffer, (success) => { Console.WriteLine(success ? "Report sent." : "Read timeout."); }, 300);

            Console.WriteLine($"{buffer.Length} bytes transmitted.");

            device.ReadReport(ReadReport_EventHandler);

            Console.ReadLine();

            device.CloseDevice();
        }

        private static void ReadReport_EventHandler(HidReport report)
        {
            Console.WriteLine($"{report.Data.Length} bytes received.");
            int length = report.Data[3] + 3;
            byte[] data = new byte[length];
            Array.Copy(report.Data, data, length);
            Console.WriteLine(BitConverter.ToString(data));
            device.ReadReport(ReadReport_EventHandler);
        }

        private static void Device_Removed()
        {
            Console.WriteLine("xArm removed.");
        }

        private static void Device_Inserted()
        {
            Console.WriteLine("xArm inserted.");
        }
    }
}
