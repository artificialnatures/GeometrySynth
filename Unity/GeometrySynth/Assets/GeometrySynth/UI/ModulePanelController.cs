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

        public bool SetModule(Connectable connectable)
        {
            titleField.text = connectable.Function.ToString();
            addressField.text = connectable.Address.ToString();
            InputValuesChanged += connectable.SyncValues;
            connectable.ModuleDataChanged += OnModuleDataChanged;
            return true;
        }
        public void SetValue(int index, int value)
        {
            if (index < values.Length)
            {
                values[index] = value;
                if (InputValuesChanged != null)
                {
                    InputValuesChanged(values);
                }
            }
        }
        public bool OnModuleDataChanged(Connectable module)
        {
            titleField.text = module.Function.ToString();
            addressField.text = module.Address.ToString();
            for (int i = 0; i < valueControls.Length; i++)
            {
                if (i < module.InputValues.Length)
                {
                    valueControls[i].SetValue(module.InputValues[i]);
                }
            }
            var upstreamText = "";
            foreach (var connection in module.UpstreamConnections)
            {
                upstreamText += connection.Address.ToString() + " ";
            }
            upstreamField.text = upstreamText;
            return true;
        }

        public event IntArrayValueChangedHandler InputValuesChanged;

        void Start()
        {
            values = new int[] { 0, 0, 0, 0 };
        }
        void Update()
        {

        }

        private int[] values;
    }
}