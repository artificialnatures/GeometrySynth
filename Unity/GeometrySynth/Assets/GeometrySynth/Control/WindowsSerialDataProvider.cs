using UnityEngine;
using System.Threading;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;

using GeometrySynth.Interfaces;

namespace GeometrySynth.Control
{
    /// <summary>
    /// Assumes a serial connection (USB) to an Arduino board acting as a relay for all connected modules.
    /// </summary>
    public class WindowsSerialDataProvider : DataProvider
    {
        public event ModuleDataChangedHandler ModuleCommandRecieved;

        public bool Connect()
        {
            if (serialPort == null)
            {
                try
                {
                    serialPort = new SerialPort(portName, portSpeed);
                    serialPort.Open();
                    if (serialPort.IsOpen)
                    {
                        isRunning = true;
                        serialThread = new Thread(SerialProcedure);
                        serialThread.Start();
                        return true;
                    }
                } catch (IOException exception) {
                    Debug.Log("SerialDataProvider: Could not open serial port. Exception: " + exception.Message);
                }
            }
            return false;
        }
        public bool Disconnect()
        {
            isRunning = false;
            serialThread.Join();
            serialPort = null;
            return false;
        }
        public bool RequestUpdate(int address)
        {
            return false;
        }
		public bool Receive()
        {
            if (ModuleCommandRecieved != null)
            {
                if (receivedQueue.Count > 0)
                {
                    for (int i = 0; i < receivedQueue.Count; i++)
                    {
                        string message = receivedQueue.Dequeue();
                        var moduleData = JsonUtility.FromJson<ModuleData>(message);
                        ModuleCommandRecieved(moduleData);
                    }
                }
            }
            return false;
        }
		public bool Send(string data)
        {
            sendQueue.Enqueue(data);
            return true;
        }

		public WindowsSerialDataProvider()
		{
            isRunning = false;
            availablePorts = FindAvailablePorts();
            portName = "COM4";
            portSpeed = 115200;
            receivedQueue = new Queue<string>();
            sendQueue = new Queue<string>();
		}

        public WindowsSerialDataProvider(string port_name, int port_speed) : this()
        {
            portName = port_name;
            portSpeed = port_speed;
        }

        private string[] FindAvailablePorts()
        {
            string[] portNames = SerialPort.GetPortNames();
            Debug.Log("SerialDataProvider - Available Serial Ports:");
            foreach (var pn in portNames)
            {
                Debug.Log("   - " + pn);
            }
            return portNames;
        }

        private void SerialProcedure()
        {
            while (isRunning)
            {
                if (serialPort.BytesToRead > 0)
                {
                    receivedQueue.Enqueue(serialPort.ReadLine());
                }
                if (sendQueue.Count > 0)
                {
                    serialPort.WriteLine(sendQueue.Dequeue());
                }
            }
        }

        private Thread serialThread;
        private SerialPort serialPort;
        private string[] availablePorts;
        private string portName;
        private int portSpeed;
        private bool isRunning;
        private Queue<string> receivedQueue;
        private Queue<string> sendQueue;
    }
}
