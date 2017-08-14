using GeometrySynth.Constants;
using GeometrySynth.Interfaces;

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
            shape = MapShape(values[0]);
            isActive = MapActive(values[1]);
            return true;
        }
        public override bool Operate(Transformable transformable)
        {
            if (transformable.IsActive != isActive)
            {
                transformable.IsActive = isActive;
            }
            if (transformable.Shape != shape)
            {
                transformable.Shape = shape;
                return true;
            }
            return false;
        }
        public Creator(int moduleAddress) : base(moduleAddress, ModuleFunction.SHAPE)
        {
            shape = Shape.CUBE;
            isActive = true;
        }
        private Shape MapShape(int inputValue)
        {
            //TODO: map a variety of shapes to input values from 0 to 255
            return Shape.CUBE;
        }
        private bool MapActive(int inputValue)
        {
            if (inputValue > 128)
            {
                return true;
            }
            return false;
        }
        private Shape shape;
        private bool isActive;
    }
}
