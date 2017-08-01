using System.Collections.Generic;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;

namespace GeometrySynth.FunctionModules
{
	public class Rotator : FunctionModule
	{
		public override bool SyncValues(int[] moduleValues)
		{
			if (moduleValues.Length == 3)
			{
                values = moduleValues;
                x = MapValue(moduleValues[0]);
                y = MapValue(moduleValues[1]);
                z = MapValue(moduleValues[2]);
				return true;
			}
			else
			{
				return false;
			}
		}
		public override bool Operate(Transformable transformable)
		{
			transformable.Rotate(x, y, z);
			return true;
		}
		public Rotator(int moduleAddress)
		{
			address = moduleAddress;
            function = ModuleFunction.ROTATE;
            values = new int[] { 0, 0, 0 };
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