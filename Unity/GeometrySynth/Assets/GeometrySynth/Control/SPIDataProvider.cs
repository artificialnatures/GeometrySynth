using UnityEngine;
using System.Threading;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;

using GeometrySynth.Interfaces;

namespace GeometrySynth.Control
{
	/// <summary>
	/// Assumes an SPI serial connection. For use with Windows IOT devices.
	/// </summary>
	public class SPIDataProvider : DataProvider
	{
		public event ModuleDataChangedHandler ModuleCommandRecieved;

		public bool Connect()
		{
			isRunning = true;
			return false;
		}
		public bool Disconnect()
		{
			isRunning = false;
			return false;
		}
		public bool RequestUpdate(int address)
		{
			return false;
		}
		public bool Receive()
		{
			//TODO: communicate with each module...
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
			//TODO: communicate with specific module...
			sendQueue.Enqueue(data);
			return false;
		}

		public SPIDataProvider()
		{
			isRunning = false;
			receivedQueue = new Queue<string>();
			sendQueue = new Queue<string>();
		}

		private void SendAndReceive()
		{
			//TODO: thread?
			while (isRunning)
			{
				//TODO: receive messages and add to receivedQueue
				//TODO: send messages from sendQueue
			}
		}

		private bool isRunning;
		private Queue<string> receivedQueue;
		private Queue<string> sendQueue;
	}
}