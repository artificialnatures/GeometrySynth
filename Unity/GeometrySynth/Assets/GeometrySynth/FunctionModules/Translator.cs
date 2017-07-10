using GeometrySynth.Constants;
using GeometrySynth.Interfaces;

namespace GeometrySynth.FunctionModules
{
    public class Translator : FunctionModule
    {
        public override bool SyncValues(int[] moduleValues)
        {
            if (moduleValues.Length == 3)
            {
                values = moduleValues;
                x = MapValue(values[0]);
                y = MapValue(values[1]);
                z = MapValue(values[2]);
                return true;
            } else {
                return false;
            }
        }
        public override bool Operate(Transformable transformable)
        {
            transformable.Translate(x, y, z);
            return true;
        }
        public Translator(int moduleAddress)
        {
            address = moduleAddress;
            function = ModuleFunction.TRANSLATE;
            values = new int[] { 0, 0, 0 };
            x = 0.0f;
            y = 0.0f;
            z = 0.0f;
        }
        private float MapValue(int rawValue)
        {
            int normalizedValue = 0;
            if (rawValue >= 128)
            {
                normalizedValue = rawValue - 128;
            } else {
                normalizedValue = -rawValue;
            }
            float mappedValue = (float)normalizedValue / 128.0f;
            return mappedValue;
        }
        private float x; //values[0]
        private float y; //values[1]
        private float z; //values[2]
    }
}