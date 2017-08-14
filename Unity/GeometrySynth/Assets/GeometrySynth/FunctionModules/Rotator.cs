using System.Collections.Generic;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;

namespace GeometrySynth.FunctionModules
{
	public class Rotator : FunctionModule
	{
		public override bool SyncValues(int[] moduleValues)
		{
            base.SyncValues(moduleValues);
            x = MapValue(values[0]);
            y = MapValue(values[1]);
            z = MapValue(values[2]);
			return true;
		}
		public override bool Operate(Transformable transformable)
		{
			transformable.Rotate(x, y, z);
			return true;
		}
		public Rotator(int moduleAddress) : base(moduleAddress, ModuleFunction.ROTATE)
		{
			x = 0.0f;
			y = 0.0f;
			z = 0.0f;
		}
		private float MapValue(int rawValue)
		{
            float mappedValue = (float)rawValue * (360.0f / 255.0f);
			return mappedValue;
		}
		float x; //values[0]
		float y; //values[1]
		float z; //values[2]
	}
}