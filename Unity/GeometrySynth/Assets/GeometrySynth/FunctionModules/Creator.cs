using GeometrySynth.Constants;
using GeometrySynth.Interfaces;
using GeometrySynth.Control;

namespace GeometrySynth.FunctionModules
{
    public class Creator : FunctionModule
    {
        public Shape Shape
        {
            get { return shape; }
        }
        public bool IsActive
        {
            get { return isActive; }
        }
        public override bool SyncValues(int[] moduleValues)
        {
            base.SyncValues(moduleValues);
            shape = (Shape)InputValueMapper.MapIntegerChoice(values[0], 0, 9);
            isActive = InputValueMapper.MapBoolean(values[1]);
            return true;
        }
        public override bool Operate(Transformable transformable)
        {
            transformable.Shape = shape;
            transformable.IsActive = isActive;
            transformable.IsArrayed = false;
            return true;
        }
        public Creator(int moduleAddress) : base(moduleAddress, ModuleFunction.SHAPE)
        {
            shape = Shape.CUBE;
            isActive = true;
        }
        private Shape shape;
        private bool isActive;
    }
}
