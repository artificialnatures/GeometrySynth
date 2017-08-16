﻿using UnityEngine;

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
                SendModuleData(arrayModule);
            }
            if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                SendModuleData(waveModule);
            }
			if (Input.GetKeyUp(KeyCode.Alpha4))
			{
                SendModuleData(rotateModule);
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
			//serialDataProvider.SendModuleData(data);
			var message = JsonUtility.ToJson(data);
			serialDataProvider.EnqueueMessage(message);
        }
        private void CreateModuleData()
        {
            shapeModule = new ModuleData()
            {
                address = 10,
                command = Command.UPDATE,
                function = ModuleFunction.SHAPE,
                values = new int[] { 1, 200, 0, 0 },
                connectedModuleAddress = 14
            };
            modules.Add(shapeModule);
			waveModule = new ModuleData()
			{
				address = 11,
                command = Command.UPDATE,
                function = ModuleFunction.SINE_WAVE,
				values = new int[] { 255, 20, 0, 0 },
				connectedModuleAddress = 15
			};
            modules.Add(waveModule);
			translateModule = new ModuleData()
			{
				address = 12,
                command = Command.UPDATE,
                function = ModuleFunction.TRANSLATE,
				values = new int[] { 255, 0, 0, 0 },
				connectedModuleAddress = 15
			};
            modules.Add(translateModule);
			colorModule = new ModuleData()
			{
				address = 13,
                command = Command.UPDATE,
                function = ModuleFunction.COLOR,
				values = new int[] { 255, 255, 255, 0 },
				connectedModuleAddress = 0
			};
            modules.Add(colorModule);
			arrayModule = new ModuleData()
			{
				address = 14,
				command = Command.UPDATE,
                function = ModuleFunction.ARRAY,
				values = new int[] { 60, 60, 60, 0 },
				connectedModuleAddress = 11
			};
			modules.Add(arrayModule);
			rotateModule = new ModuleData()
			{
				address = 15,
				command = Command.UPDATE,
                function = ModuleFunction.ROTATE,
				values = new int[] { 100, 0, 255, 0 },
				connectedModuleAddress = 0
			};
			modules.Add(translateModule);
        }
        private SerialDataProvider serialDataProvider;
        private List<ModuleData> modules;
        private ModuleData shapeModule;
        private ModuleData waveModule;
        private ModuleData translateModule;
        private ModuleData rotateModule;
        private ModuleData colorModule;
        private ModuleData arrayModule;
    }
}