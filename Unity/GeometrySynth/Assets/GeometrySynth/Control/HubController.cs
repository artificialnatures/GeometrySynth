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
            //hub.AddDataProvider(new SerialDataProvider()); //TODO: Causes crash, need to debug...

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