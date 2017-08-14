using UnityEngine;
using UnityEngine.UI;

using GeometrySynth.Interfaces;

namespace GeometrySynth.UI
{
    public class ValueControl : MonoBehaviour
    {
        public ModulePanelController module;
        public int valueIndex;
        public Button button;
        public Slider slider;
        public Text label;

        public void SetLabel(string labelText)
        {
            label.text = labelText;
        }
        public void OnValueChanged(float floatValue)
        {
            /* TODO: enable once manual controls are figured out...
            if (module != null)
            {
                inputValue = (int)floatValue;
                module.SetValue(valueIndex, inputValue);
                label.text = inputValue.ToString();
            }
            */
        }
        public void SetValue(int intValue)
        {
            if (slider != null)
            {
                float floatValue = (float)intValue;
                slider.value = floatValue;
                label.text = floatValue.ToString();
            }
        }
        public void Trigger()
        {
            
        }

        void Start()
        {
            if (module != null)
            {
                
            }
        }
        void Update()
        {

        }

        private int inputValue;
    }
}