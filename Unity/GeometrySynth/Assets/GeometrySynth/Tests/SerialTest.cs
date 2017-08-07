using UnityEngine;

using System.Collections.Generic;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;
using GeometrySynth.Control;

namespace GeometrySynth.Tests
{
    public class SerialTest
    {
        public void HandleKeyboardInput()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1)) 
            {
                SendModuleData(shapeModule);
            }
            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                SendModuleData(waveModule);
            }
            if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                SendModuleData(translateModule);
            }
            if (Input.GetKeyUp(KeyCode.Alpha4))
            {
                SendModuleData(colorModule);
            }
        }
        public bool OnModuleCreated(Connectable connectable)
        {
            bool didUpdate = false;
            foreach (var module in modules)
            {
                var mod = module;
                if (connectable.Address == mod.address)
                {
                    mod.command = Command.UPDATE;
                    didUpdate = true;
                }
            }
            return didUpdate;
        }
        public SerialTest(SerialDataProvider provider)
        {
            serialDataProvider = provider;
            modules = new List<ModuleData>();
            CreateModuleData();
        }
        public void SendModuleData(ModuleData data)
        {
            serialDataProvider.SendModuleData(data);
			if (data.command == Command.CONNECT)
			{
				data.command = Command.LINK;
			}
			else if (data.command == Command.LINK)
			{
				data.command = Command.UPDATE;
			}
        }
        private void CreateModuleData()
        {
            shapeModule = new ModuleData()
            {
                address = 10,
                command = Command.CONNECT,
                function = ModuleFunction.SHAPE,
                values = new int[] { 1 },
                connectedModuleAddress = 12
            };
            modules.Add(shapeModule);
			waveModule = new ModuleData()
			{
				address = 11,
                command = Command.CONNECT,
                function = ModuleFunction.SINE_WAVE,
				values = new int[] { 500, 500 },
				connectedModuleAddress = 1
			};
            modules.Add(waveModule);
			translateModule = new ModuleData()
			{
				address = 12,
                command = Command.CONNECT,
                function = ModuleFunction.TRANSLATE,
				values = new int[] { 1023, 0, 0 },
				connectedModuleAddress = 11
			};
            modules.Add(translateModule);
			colorModule = new ModuleData()
			{
				address = 13,
                command = Command.CONNECT,
                function = ModuleFunction.COLOR,
				values = new int[] { 1023, 1023, 1023 },
				connectedModuleAddress = 0
			};
            modules.Add(colorModule);
        }
        private SerialDataProvider serialDataProvider;
        private List<ModuleData> modules;
        private ModuleData moduleToSend;
        private ModuleData shapeModule;
        private ModuleData waveModule;
        private ModuleData translateModule;
        private ModuleData colorModule;
    }
}