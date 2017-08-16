using System.Collections.Generic;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;
using GeometrySynth.Control;

namespace GeometrySynth.FunctionModules
{
	public class Scaler : FunctionModule
	{
		public override bool SyncValues(int[] moduleValues)
		{
			base.SyncValues(moduleValues);
            x = InputValueMapper.MapScale(values[0]);
            y = InputValueMapper.MapScale(values[1]);
            z = InputValueMapper.MapScale(values[2]);
			return true;
		}
		public override bool Operate(Transformable transformable)
		{
			transformable.Scale(x, y, z);
			return true;
		}
        public Scaler(int moduleAddress) : base(moduleAddress, ModuleFunction.SCALE)
		{
			x = 1.0f;
			y = 1.0f;
			z = 1.0f;
		}
		float x; //values[0]
		float y; //values[1]
		float z; //values[2]
	}
}