﻿using UnityEngine;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;
using GeometrySynth.UI;

namespace GeometrySynth.Control
{
    public class HubController : MonoBehaviour
    {
        public SceneController sceneController;
        public ScreenControls screenControls;
        public string serialPortName;
        public int serialBaudRate;

        public bool OnModuleCreated(Connectable module)
        {
            screenControls.AddModulePanel(module);
            return false;
        }
        public bool OnLogMessageReceived(string message)
        {
            Debug.Log(message);
            return false;
        }

        void Start()
        {
			if (sceneController != null)
			{
                hub = new Hub(sceneController);
	            hub.AddDataProvider(screenControls);
	            serialDataProvider = new SerialDataProvider("Arduino", 9600);
	            hub.AddDataProvider(serialDataProvider);
	            hub.LogMessageGenerated += OnLogMessageReceived;
	            hub.ModuleCreated += OnModuleCreated;
                hub.ShapeModuleCreated += sceneController.OnShapeModuleCreated;
				serialTest = new Tests.SerialTest(serialDataProvider);
				hub.ModuleCreated += serialTest.OnModuleCreated;
            }
        }
        void Update()
        {
            HandleKeyboardInput();
            if (serialTest != null) serialTest.HandleKeyboardInput();
            if (hub != null)
            {
                hub.Sync();
                hub.Step(Time.realtimeSinceStartup);
                hub.Operate();
            }
        }
        void OnApplicationQuit()
        {
            hub.Disconnect();
        }

        private void HandleKeyboardInput()
        {
			
        }

        private Hub hub;
        private SerialDataProvider serialDataProvider;
        private Tests.SerialTest serialTest;
    }
}