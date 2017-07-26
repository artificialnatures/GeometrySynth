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
            if (module != null)
            {
                inputValue = (int)floatValue;
                module.SetValue(valueIndex, inputValue);
                label.text = inputValue.ToString();
            }
        }
        public void SetValue(float floatValue)
        {
            if (slider != null)
            {
                slider.value = floatValue;
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