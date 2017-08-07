using UnityEngine;
using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

using GeometrySynth.Interfaces;

namespace GeometrySynth.Control
{
	/// <summary>
	/// Cross-platform serial communications based on:
	/// http://wjwwood.github.com/serial/
	/// Assumes a serial connection (USB) to an Arduino board acting as a relay for all connected modules.
	/// </summary>
	public class SerialDataProvider : DataProvider
	{
		public event ModuleDataChangedHandler ModuleCommandRecieved;

		[DllImport("wjwwoodserial")]
		private static extern bool serial_refresh_device_list();
		[DllImport("wjwwoodserial")]
		private static extern int serial_device_count();
		[DllImport("wjwwoodserial")]
		private static extern IntPtr serial_get_device_path_for_index(int device_index);
		[DllImport("wjwwoodserial")]
		private static extern bool serial_set_device_path(IntPtr path);
		[DllImport("wjwwoodserial")]
        private static extern IntPtr serial_get_device_path();
        [DllImport("wjwwoodserial")]
        private static extern bool serial_set_device_path_by_index(int device_index);
		[DllImport("wjwwoodserial")]
		private static extern bool serial_set_speed(int speed);
		[DllImport("wjwwoodserial")]
		private static extern int serial_get_speed();
		[DllImport("wjwwoodserial")]
		private static extern bool serial_set_timeout_milliseconds(int timeout);
		[DllImport("wjwwoodserial")]
		private static extern int serial_get_timeout_milliseconds();
		[DllImport("wjwwoodserial")]
		private static extern bool serial_open();
		[DllImport("wjwwoodserial")]
		private static extern bool serial_is_open();
		[DllImport("wjwwoodserial")]
		private static extern bool serial_data_available();
		[DllImport("wjwwoodserial")]
		private static extern int serial_data_available_count();
		[DllImport("wjwwoodserial")]
		private static extern bool serial_send(IntPtr data_to_send);
		[DllImport("wjwwoodserial")]
		private static extern bool serial_receive();
		[DllImport("wjwwoodserial")]
		private static extern int serial_available_messages_count();
		[DllImport("wjwwoodserial")]
		private static extern int serial_get_message_length();
		[DllImport("wjwwoodserial")]
		private static extern IntPtr serial_get_next_message();
		[DllImport("wjwwoodserial")]
		private static extern bool serial_close();

		public bool Connect()
		{
            portSpeed = 19200;
            bool refreshed = serial_refresh_device_list();
            if (refreshed)
            {
                int deviceCount = serial_device_count();
                if (deviceCount > 0)
                {
                    int portIndex = -1;
                    Debug.Log("There are " + deviceCount.ToString() + " serial devices available.");
                    for (int i = 0; i < deviceCount; i++)
                    {
                        IntPtr devicePathPointer = serial_get_device_path_for_index(i);
                        string devicePath = Marshal.PtrToStringAnsi(devicePathPointer);
                        if (devicePath != null)
                        {
                            //TODO: detect path in a cross platform way, or some kind of hand shaking...
                            if (devicePath.Contains("usbmodem")) //on a mac, the path will be something like: /dev/cu.usbmodem1411
                            {
                                portName = devicePath;
                                portIndex = i;
                                Debug.Log("Device " + i.ToString() + ": " + devicePath + "\nGuessing that this one is the Arduino.");
                            }
                            else
                            {
                                Debug.Log("Device " + i.ToString() + ": " + devicePath);
                            }
                        }
                    }
                    if (portIndex > -1)
                    {
                        serial_set_device_path_by_index(portIndex);
                        IntPtr ptr = serial_get_device_path();
                        Debug.Log("Set device path to " + Marshal.PtrToStringAnsi(ptr));
                        serial_set_speed(portSpeed);
                        Debug.Log("Set baud rate to " + serial_get_speed().ToString());
                        serial_set_timeout_milliseconds(1000);
                        Debug.Log("Set timeout to " + serial_get_timeout_milliseconds().ToString());
                        Debug.Log("Attempting to open serial port: " + portName);
                        bool didOpen = serial_open();
                        if (didOpen)
                        {
                            Debug.Log("Opened serial port: " + portName);
							receiveThread = new Thread(ReceiveLoop);
							receiveThread.Start();
                            Debug.Log("Receive thread running.");
                            return true;
                        }
                        else
                        {
                            Debug.Log("Failed to open serial port.");
                        }
                    }
                } else {
                    Debug.Log("No serial devices found.");
                }
            }
			return false;
		}

		public bool Disconnect()
		{
			serial_close();
			isRunning = false;
			if (sendThread != null)
			{
				sendThread.Join();
			}
			if (receiveThread != null)
			{
				receiveThread.Join();
			}
			return true;
		}

		public bool RequestUpdate(int address)
		{
			return false;
		}

		public bool Receive()
		{
            if (receivedQueue.Count > 0)
			{
				string message;
                receivedQueue.TryDequeue(out message);
                Debug.Log("Received message: " + message);
                var moduleData = JsonUtility.FromJson<ModuleData>(message);
                Debug.Log("Received module data from address " + moduleData.address.ToString() + ".");
                if (ModuleCommandRecieved != null) ModuleCommandRecieved(moduleData);
                return true;
			}
			return false;
		}

		public bool Send(string data)
		{
            if (sendQueue.Count > 0)
            {
                return true;
            }
            return false;
		}

        public bool SendModuleData(ModuleData data)
        {
            string message = ModuleDataToJSON(data) + EOL;
            IntPtr ptr = Marshal.StringToHGlobalAnsi(message);
			serial_send(ptr);
            Marshal.FreeHGlobal(ptr);
            return true;
        }

		public SerialDataProvider()
		{
			isRunning = false;
			portName = "";
			portSpeed = 19200;
			receivedQueue = new ConcurrentQueue<string>();
			sendQueue = new ConcurrentQueue<string>();
		}

		public SerialDataProvider(string port_name, int port_speed) : this()
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

        private string ModuleDataToJSON(ModuleData data)
        {
            string json = "{";
            json += "\"address\":" + data.address.ToString() + ",";
            json += "\"command\":" + ((int)data.command).ToString() + ",";
            json += "\"function\":" + ((int)data.function).ToString() + ",";
            json += "\"values\":[";
            for (int i = 0; i < data.values.Length; i++)
            {
                json += data.values[i].ToString();
                if (i != data.values.Length - 1)
                {
                    json += ", ";
                }
            }
            json += "],";
            json += "\"connectedModuleAddress\":" + ((int)data.connectedModuleAddress).ToString();
            json += "}";
            return json;
        }

        private void SendLoop()
        {
            while (isRunning)
            {
				if (sendQueue.Count > 0)
				{
					string messageToSend;
					sendQueue.TryDequeue(out messageToSend);
                    IntPtr ptr = Marshal.StringToHGlobalAnsi(messageToSend);
					serial_send(ptr);
                    Marshal.FreeHGlobal(ptr);
				}
                Thread.Sleep(10);
            }
        }

        private void ReceiveLoop()
        {
            isRunning = true;
            while(isRunning)
            {
				if (serial_receive())
				{
                    int messageCount = serial_available_messages_count();
                    if (messageCount > 0)
                    {
                        Debug.Log(messageCount.ToString() + " messages available.");
                        IntPtr ptr = serial_get_next_message();
                        string receivedMessage = Marshal.PtrToStringAnsi(ptr);
                        if (receivedMessage == null)
                        {
                            Debug.Log("Received null message.");
                        } else {
                            Debug.Log("Received message: " + receivedMessage);
                            if (receivedMessage.Contains("{") && receivedMessage.Contains("}"))
                            {
                                receivedQueue.Enqueue(receivedMessage);
                            }
                        }
                    }
                }
                Thread.Sleep(10);
            }
        }

		private Thread sendThread;
        private Thread receiveThread;
		private string[] availablePorts;
		private string portName;
		private int portSpeed;
		private bool isRunning;
		private ConcurrentQueue<string> receivedQueue;
		private ConcurrentQueue<string> sendQueue;
        private readonly string EOL = "\n";
	}
}