using UnityEngine;
using System.Threading;
using System.IO.Ports;
using System.Collections.Generic;

using GeometrySynth.Interfaces;

namespace GeometrySynth.Control
{
    /// <summary>
    /// Assumes a serial connection (USB) to an Arduino board acting as a relay for all connected modules.
    /// </summary>
    public class SerialDataProvider : DataProvider
    {
        public event ModuleDataChangedHandler ModuleCommandRecieved;

        public bool Connect()
        {
            if (serialPort == null)
            {
                serialPort = new SerialPort("COM4", 9600);
                serialPort.Open();
                if (serialPort.IsOpen)
                {
                    isRunning = true;
                    serialThread = new Thread(SerialProcedure);
                    serialThread.Start();
                    return true;
                }
            }
            return false;
        }
        public bool Disconnect()
        {
            isRunning = false;
            serialThread.Join();
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
		public bool Send(ModuleData moduleData)
        {
            var message = JsonUtility.ToJson(moduleData);
            sendQueue.Enqueue(message);
            return false;
        }

		public SerialDataProvider()
		{
            isRunning = false;
            receivedQueue = new Queue<string>();
            sendQueue = new Queue<string>();
		}

        private static void SerialProcedure()
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

        private static Thread serialThread;
        private static SerialPort serialPort;
        private static bool isRunning;
        private static Queue<string> receivedQueue;
        private static Queue<string> sendQueue;
    }
}
