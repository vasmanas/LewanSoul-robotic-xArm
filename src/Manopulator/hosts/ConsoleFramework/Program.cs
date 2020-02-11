namespace ConsoleFramework
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using ConsoleFramework.Manipulator;
    using HidLibrary;

    public class Program
    {
        static async Task Main(string[] args)
        {
            var device = new Controller();

            await device.MultiServoMove(1000, servo1: 0);
            await device.MultiServoMove(1000, servo1: 650);
            await device.MultiServoMove(1000, servo1: 650);
            await device.MultiServoMove(1000, servo1: 650);
            await device.MultiServoMove(1000, servo1: 650);

            var buffer = new byte[device.Device.Capabilities.OutputReportByteLength];

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

            device.Device.Write(buffer, (success) => { Console.WriteLine(success ? "Report sent." : "Read timeout."); }, 300);

            Console.WriteLine($"{buffer.Length} bytes transmitted.");

            //device.Device.ReadReport(ReadReport_EventHandler);

            Console.ReadLine();
        }

        //private static void ReadReport_EventHandler(HidReport report)
        //{
        //    Console.WriteLine($"{report.Data.Length} bytes received.");
        //    int length = report.Data[3] + 3;
        //    byte[] data = new byte[length];
        //    Array.Copy(report.Data, data, length);
        //    Console.WriteLine(BitConverter.ToString(data));
        //    device.ReadReport(ReadReport_EventHandler);
        //}
    }
}