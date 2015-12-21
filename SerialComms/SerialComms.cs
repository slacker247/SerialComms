using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace SerialComms
{
    public class SerialComms
    {
        protected SerialPort comPort;

        public delegate void Message(String message);
        public event Message msg;

        public delegate void recv(object data);
        public event recv received;

        public SerialComms()
        {
            init();
        }

        ~SerialComms()
        {
            comPort.Close();
        }

        public int init()
        {
            int status = -1;
            comPort = comPort = new SerialPort();
            comPort.DataReceived += new SerialDataReceivedEventHandler(dataReceived);
            return status;
        }

        #region Getters and Setters

        public int getBaudRate()
        {
            return comPort.BaudRate;
        }

        public int setBaudRate(int baud)
        {
            int status = -1;
            comPort.BaudRate = baud;
            return status;
        }

        public int getDataBits()
        {
            return comPort.DataBits;
        }

        public int setDataBits(int bits)
        {
            int status = -1;
            comPort.DataBits = bits;
            return status;
        }

        public StopBits getStopBits()
        {
            return comPort.StopBits;
        }

        public int setStopBits(StopBits bits)
        {
            int status = -1;
            comPort.StopBits = bits;
            return status;
        }

        public int setStopBits(String stopBits)
        {
            int status = -1;
            setStopBits((StopBits)Enum.Parse(typeof(StopBits), stopBits));
            return status;
        }

        public Parity getParity()
        {
            return comPort.Parity;
        }

        public int setParity(Parity parity)
        {
            int status = -1;
            comPort.Parity = parity;
            return status;
        }

        public int setParity(String parity)
        {
            int status = -1;
            setParity((Parity)Enum.Parse(typeof(Parity), parity));
            return status;
        }

        public String getPortName()
        {
            return comPort.PortName;
        }

        public int setPortName(String port)
        {
            int status = -1;
            try
            {
                comPort.PortName = port;
                status = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid port name.", ex.Message);
                if (msg != null)
                    msg("Invalid port name.");
            }
            return status;
        }

        public bool isOpen()
        {
            bool status = false;
            if (comPort != null)
                status = comPort.IsOpen;
            return status;
        }

        public static String[] getComPorts()
        {
            return SerialPort.GetPortNames();
        }

        public static String[] getParityValues()
        {
            return Enum.GetNames(typeof(Parity));
        }

        public static String[] getStopBitValues()
        {
            return Enum.GetNames(typeof(StopBits));
        }

        #endregion

        public int open()
        {
            int status = -1;
            try
            {
                //first check if the port is already open
                //if its open then close it
                if (comPort.IsOpen == true)
                    comPort.Close();

                //set the properties of our SerialPort Object
                //comPort.BaudRate = getBaudRate();    //BaudRate
                //comPort.DataBits = getDataBits();    //DataBits
                //comPort.StopBits = getStopBits();    //StopBits
                //comPort.Parity = getParity();    //Parity
                //comPort.PortName = getPortName();   //PortName

                //now open the port
                comPort.Open();
                status = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to open port.", ex.Message);
                if (msg != null)
                    msg("Failed to open port.");
            }
            return status;
        }

        public int close()
        {
            int status = -1;
            comPort.Close();
            return status;
        }

        public int sendData(String data)
        {
            int status = -1;
            byte[] bytes = new byte[data.Length];
            for (int i = 0; i < data.Length; i++ )
                bytes[i] = Convert.ToByte(data[i]);
            status = sendData(bytes);
            return status;
        }

        public int sendData(int data)
        {
            return sendData(BitConverter.GetBytes(data));
        }

        public int sendData(byte[] data)
        {
            int status = -1;
            //send the message to the port
            comPort.Write(data, 0, data.Length);
            return status;
        }

        protected void dataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int numBytes = comPort.BytesToRead;
            //create a byte array to hold the awaiting data
            byte[] comBuffer = new byte[numBytes];
            //read the data and store it
            comPort.Read(comBuffer, 0, numBytes);

            if (received != null)
                received(comBuffer);
        }
    }
}
