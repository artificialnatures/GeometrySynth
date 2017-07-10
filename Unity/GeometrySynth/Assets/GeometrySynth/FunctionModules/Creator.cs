using GeometrySynth.Constants;
using GeometrySynth.Interfaces;

namespace GeometrySynth.FunctionModules
{
    public class Creator : FunctionModule
    {
        public override bool Operate(Transformable transformable)
        {
            if (transformable.Shape != shape)
            {
                transformable.Shape = shape;
                return true;
            }
            return false;
        }
        public Creator(int moduleAddress)
        {
            address = moduleAddress;
            function = ModuleFunction.SHAPE;
            shape = Creation.CUBE;
        }
        private Creation shape;
    }
}
