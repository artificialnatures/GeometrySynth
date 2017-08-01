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
            /*
            SerialDataProvider sdp = null;
            if (serialPortName != "" && serialBaudRate > 0)
            {
                sdp = new SerialDataProvider(serialPortName, serialBaudRate);
            } else {
                sdp = new SerialDataProvider();
            }
            if (sdp != null)
            {
                hub.AddDataProvider(sdp);
            }
            */
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            if (ports.Length > 0)
            {
                foreach (var pn in ports)
                {
                    Debug.Log("HubController: Found available serial port: " + pn);
                }
            } else {
                Debug.Log("HubController: Found no available serial ports.");
            }
            hub.LogMessageGenerated += OnLogMessageReceived;
            hub.ModuleCreated += OnModuleCreated;
            /*
            var createModule = hub.ConnectModule(4, ModuleFunction.CREATE);
            var waveModule = hub.ConnectModule(5, ModuleFunction.SINE_WAVE);
			var translateModule = hub.ConnectModule(7, ModuleFunction.TRANSLATE);
            var rotateModule = hub.ConnectModule(6, ModuleFunction.ROTATE);
            var scaleModule =  hub.ConnectModule(9, ModuleFunction.SCALE);
            var colorModule = hub.ConnectModule(8, ModuleFunction.COLOR);
            screenControls.AddModulePanel(waveModule);
            screenControls.AddModulePanel(translateModule);
            screenControls.AddModulePanel(rotateModule);
            screenControls.AddModulePanel(scaleModule);
            screenControls.AddModulePanel(colorModule);
            */
        }
        void Update()
        {
            hub.Sync();
            hub.Step(Time.realtimeSinceStartup);
            hub.Operate(testNode);
        }
        private Hub hub;
        private SceneNode testNode;
    }
}