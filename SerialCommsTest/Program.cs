using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SerialCommsTest
{
    class Program
    {
        public static int high = 0;

        static void received(object data)
        {
            if (data is byte[])
            {
                //Console.Write("0x");
                //int value = -1;
                //foreach (byte b in (byte[])data)
                //{
                //    value = Convert.ToInt32(b);
                //    value = value & 0x0F;
                //    if (value > 7)
                //        value = value >> 1;
                //    string hexOutput = String.Format("{0:X}", value);
                //    Console.Write(hexOutput);
                //}
                //Console.WriteLine();

                String buffer = System.Text.Encoding.Default.GetString((byte[])data);
                Console.WriteLine(buffer);
                if (buffer.StartsWith("Buffer:"))
                {
                    buffer = buffer.Substring(7);
                    Console.WriteLine(String.Format("{0:X}", buffer));
                }

                //Console.WriteLine("String: " + ASCIIEncoding.ASCII.GetString((byte[])data));
                //if (value == high)
                //    high++;
                //if (high > 15)
                //    high = 0;
            }
        }

        static void consoleTest()
        {
            bool test = true;
            int count = 0;
            byte[] command = new byte[1];
            command[0] = 0x01;

            SerialComms.SerialComms comPort = new SerialComms.SerialComms();
            comPort.setBaudRate(9600);    //BaudRate
            comPort.setPortName("COM8");   //PortName
            comPort.received += new SerialComms.SerialComms.recv(received);
            comPort.open();
            Thread.Sleep(10);
            high = 1;
            Console.WriteLine("High = " + high);
            comPort.sendData(high + "\r\n");

            while (test)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    test = false;
                }
                Thread.Sleep(150);
                if (count >= 5000f/150f)
                {
                    Console.WriteLine("High = " + high);
                    comPort.sendData(high + "\r\n");
                    //high++;
                    //if (high >= 16)
                    //    high = 0;
                    count = 0;
                }
                count++;
            }
        }

        static void Main(string[] args)
        {
            consoleTest();
        }
    }
}
