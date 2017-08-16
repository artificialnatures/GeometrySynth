using GeometrySynth.Constants;
using GeometrySynth.Interfaces;
using GeometrySynth.Control;

namespace GeometrySynth.FunctionModules
{
    public class Translator : FunctionModule
    {
        public override bool SyncValues(int[] moduleValues)
        {
            base.SyncValues(moduleValues);
            x = InputValueMapper.MapTranslation(values[0]);
	        y = InputValueMapper.MapTranslation(values[1]);
	        z = InputValueMapper.MapTranslation(values[2]);
	        return true;
        }
        public override bool Operate(Transformable transformable)
        {
            transformable.Translate(x, y, z);
            return true;
        }
        public Translator(int moduleAddress) : base(moduleAddress, ModuleFunction.TRANSLATE)
        {
            x = 0.0f;
            y = 0.0f;
            z = 0.0f;
        }
        private float x; //values[0]
        private float y; //values[1]
        private float z; //values[2]
    }
}