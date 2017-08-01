using System.Collections.Generic;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;

namespace GeometrySynth.FunctionModules
{
	public class Colorer : FunctionModule
	{
		public override bool SyncValues(int[] moduleValues)
		{
			if (moduleValues.Length == 3)
			{
                values = moduleValues;
                r = MapValue(values[0]);
                g = MapValue(values[1]);
                b = MapValue(values[2]);
				return true;
			}
			else
			{
				return false;
			}
		}
		public override bool Step(float time)
		{
			return false;
		}
		public override bool Operate(Transformable transformable)
		{
			transformable.Color(r, g, b);
			return true;
		}
        public Colorer(int moduleAddress)
		{
			address = moduleAddress;
            function = ModuleFunction.COLOR;
            values = new int[] { 255, 255, 255 };
			r = 1.0f;
			g = 1.0f;
			b = 1.0f;
		}
		private float MapValue(int rawValue)
		{
			float mappedValue = (float)rawValue / 255.0f;
			return mappedValue;
		}
		float r; //values[0]
		float g; //values[1]
		float b; //values[2]
	}
}