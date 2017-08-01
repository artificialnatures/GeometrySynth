using UnityEngine;
using UnityEngine.UI;

using GeometrySynth.Constants;
using GeometrySynth.Control;
using GeometrySynth.Interfaces;

namespace GeometrySynth.UI
{
    public class ModulePanelController : MonoBehaviour
    {
        public Text titleField;
        public Text addressField;
        public Text upstreamField;
        public Text downstreamField;
        public ValueControl[] valueControls;

        public int Address
        {
            get { return moduleData.address; }
        }

        public bool SetModule(Connectable connectable)
        {
            titleField.text = connectable.Function.ToString();
            addressField.text = connectable.Address.ToString();
            moduleData = new ModuleData()
            {
                address = connectable.Address,
                function = connectable.Function,
                command = Command.UPDATE,
                values = connectable.InputValues,
                connectedModuleAddress = 0
            };
            for (int i = 0; i < valueControls.Length; i++)
            {
                if (i > moduleData.values.Length - 1) 
                {
                    valueControls[i].gameObject.SetActive(false);
                } else {
                    valueControls[i].gameObject.SetActive(true);
                }
            }
            InputValuesChanged += connectable.SyncValues;
            connectable.ModuleDataChanged += OnModuleDataChanged;
            return true;
        }
        public void SetValue(int index, int value)
        {
            if (moduleData.values.Length > index)
            {
                moduleData.values[index] = value;
                if (InputValuesChanged != null)
                {
                    InputValuesChanged(moduleData.values);
                }
            }
        }
        public void TriggerEvent(int index)
        {
            
        }
        public bool OnModuleDataChanged(ModuleData updatedData)
        {
            if (updatedData.values.Length == moduleData.values.Length)
            {
                moduleData.values = updatedData.values;
                for (int i = 0; i < moduleData.values.Length; i++)
                {
                    valueControls[0].SetValue(moduleData.values[i]);
                }
                return true;
            } else {
                return false;
            }
        }

        public event ModuleDataChangedHandler ModuleDataChanged;
        public event IntArrayValueChangedHandler InputValuesChanged;

        void Start()
        {
            
        }
        void Update()
        {

        }

        private ModuleData moduleData;
    }
}