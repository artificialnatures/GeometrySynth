using UnityEngine;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;
using GeometrySynth.UI;

namespace GeometrySynth.Control
{
    public class HubController : MonoBehaviour
    {
        public ScreenControls screenControls;
        public GameObject cubePrefab;
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
            var sceneObject = new GameObject("SceneNode");
            testNode = sceneObject.AddComponent<SceneNode>();
            testNode.Create(cubePrefab);

            hub = new Hub();
            hub.AddDataProvider(screenControls);
            serialDataProvider = new SerialDataProvider("Arduino", 19200);
            hub.AddDataProvider(serialDataProvider);

            hub.LogMessageGenerated += OnLogMessageReceived;
            hub.ModuleCreated += OnModuleCreated;

            serialTest = new Tests.SerialTest(serialDataProvider);
            hub.ModuleCreated += serialTest.OnModuleCreated;
        }
        void Update()
        {
            HandleKeyboardInput();
            if (serialTest != null) serialTest.HandleKeyboardInput();
            if (hub != null)
            {
                hub.Sync();
                hub.Step(Time.realtimeSinceStartup);
                hub.Operate(testNode);
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
        private SceneNode testNode;
        private SerialDataProvider serialDataProvider;
        private Tests.SerialTest serialTest;
    }
}