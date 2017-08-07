using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using GeometrySynth.Interfaces;

namespace GeometrySynth.Control
{
    /// <summary>
    /// Assumes a serial connection (USB) to an Arduino board acting as a relay for all connected modules.
    /// </summary>
    public class MacSerialDataProvider : DataProvider
    {
        public event ModuleDataChangedHandler ModuleCommandRecieved;

        [DllImport("SerialConnection")]
        private static extern int SerialGetBaudRate();
        [DllImport("SerialConnection")]
        private static extern bool SerialSetBaudRate(int baud_rate);
        [DllImport("SerialConnection")]
        private static extern int SerialGetDeviceCount();
        [DllImport("SerialConnection")]
        private static extern IntPtr SerialGetDeviceName(int device_index);
		[DllImport("SerialConnection")]
		private static extern IntPtr SerialGetDevicePath(int device_index);
        [DllImport("SerialConnection")]
        private static extern bool SerialSetActiveDevice(int device_index);
        [DllImport("SerialConnection")]
        private static extern bool SerialOpenConnection();
        [DllImport("SerialConnection")]
        private static extern bool SerialCloseConnection();
        [DllImport("SerialConnection")]
        private static extern bool SerialSendMessage(IntPtr message);
        [DllImport("SerialConnection")]
        private static extern bool SerialMessagesAvailable();
        [DllImport("SerialConnection")]
        private static extern IntPtr SerialDequeueReceivedMessage();


		public bool Connect()
        {
			/* if serial port available...
            serialThread = new Thread(SerialProcedure);
            serialThread.Start();
            return true;
            */
			/*
			string outputString = "Serial devices found:\n";
            int serialDeviceCount = SerialGetDeviceCount();
            if (serialDeviceCount > 0)
            {
                for (int i = 0; i < serialDeviceCount; i++)
                {
                    outputString = outputString + SerialGetDeviceName(i) + "\n";
                }
            }
            else
            {
                outputString = outputString + "None found.";
            }
            Debug.Log(outputString);
            */

			SerialSetBaudRate(9600);
			Debug.Log("Serial baud rate = " + SerialGetBaudRate().ToString());
            int serialDeviceCount = SerialGetDeviceCount();
            int probableArduinoIndex = -1;
            string outputString = "There are " + serialDeviceCount.ToString() + " serial devices available:\n";
            for (int i = 0; i < serialDeviceCount; i++)
            {
                IntPtr namePointer = SerialGetDeviceName(i);
                string deviceName = Marshal.PtrToStringAnsi(namePointer);
                if (deviceName.Contains("usbmodem"))
                {
                    probableArduinoIndex = i;
                }
                outputString = outputString + "  - " + deviceName + "\n";
            }
            Debug.Log(outputString);
            if (probableArduinoIndex > -1)
            {
                Debug.Log("Guessing that the Arduino is at index " + probableArduinoIndex.ToString() + ".");
                bool activeDeviceSet = SerialSetActiveDevice(probableArduinoIndex);
                if (activeDeviceSet)
                {
                    SerialOpenConnection();
                    sendQueue.Enqueue("hello.");
                    sendQueue.Enqueue("what up?");
                    sendQueue.Enqueue("bye bye.");
                    serialThread = new Thread(SendAndReceive);
					serialThread.Start();
                    return true;
                }
            }
            return false;
        }
        public bool Disconnect()
        {
            SerialCloseConnection();
            isRunning = false;
            if (serialThread.IsAlive)
            {
                serialThread.Join();
            }
            return true;
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

        public MacSerialDataProvider()
        {
            isRunning = false;
            portName = "";
            portSpeed = 115200;
            receivedQueue = new Queue<string>();
            sendQueue = new Queue<string>();
        }

        public MacSerialDataProvider(string port_name, int port_speed) : this()
        {
            portName = port_name;
            portSpeed = port_speed;
        }

        private string[] FindAvailablePorts()
        {
            string[] portNames = {};
            Debug.Log("SerialDataProvider - Available Serial Ports:");
            foreach (var pn in portNames)
            {
                Debug.Log("   - " + pn);
            }
            return portNames;
        }

        private void SendAndReceive()
        {
            while (isRunning)
            {
                if (sendQueue.Count > 0)
                {
                    SerialSendMessage(Marshal.StringToBSTR(sendQueue.Dequeue()));
                }
                if (SerialMessagesAvailable())
                {
					IntPtr receivedMessagePointer = SerialDequeueReceivedMessage();
					string receivedMessage = Marshal.PtrToStringAnsi(receivedMessagePointer);
                    receivedQueue.Enqueue(receivedMessage);
                }
                Thread.Sleep(20);
            }
        }

        private Thread serialThread;
        private string[] availablePorts;
        private string portName;
        private int portSpeed;
        private bool isRunning;
        private Queue<string> receivedQueue;
        private Queue<string> sendQueue;
    }
}
