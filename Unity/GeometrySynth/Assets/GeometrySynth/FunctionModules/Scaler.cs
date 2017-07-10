using System.Collections.Generic;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;

namespace GeometrySynth.FunctionModules
{
	public class Scaler : FunctionModule
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
			}
			else
			{
				return false;
			}
		}
		public override bool Operate(Transformable transformable)
		{
			transformable.Scale(x, y, z);
			return true;
		}
		public Scaler(int moduleAddress)
		{
			address = moduleAddress;
            function = ModuleFunction.SCALE;
            values = new int[] { 0, 0, 0 };
			x = 1.0f;
			y = 1.0f;
			z = 1.0f;
		}
		private float MapValue(int rawValue)
		{
            float mappedValue = (float)rawValue / 255.0f;
			return mappedValue;
		}
		float x; //values[0]
		float y; //values[1]
		float z; //values[2]
	}
}