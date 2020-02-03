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

        static void Main(string[] args)
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

                bw.Write((byte)5); // Length
                bw.Write((byte)3); // Command: MultiServoMove = 3

                bw.Write((byte)1); // (byte)count
                bw.Write((ushort)1); // (ushort)milliseconds
                bw.Write((byte)4); // (byte)servo

                //bw.Write((byte)0);
                //bw.Write((ushort)0x5555);

                //bw.Write((byte)5);
                //bw.Write((byte)6);

                //bw.Write((byte)0);
                //bw.Write((ushort)2);
            }

            device.Write(buffer, (success) => { Console.WriteLine(success ? "Report sent." : "Read timeout."); }, 300);

            //Thread.Sleep(300);
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
