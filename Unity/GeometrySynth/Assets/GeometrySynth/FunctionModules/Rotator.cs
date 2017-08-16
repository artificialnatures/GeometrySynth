using System.Collections.Generic;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;
using GeometrySynth.Control;

namespace GeometrySynth.FunctionModules
{
	public class Rotator : FunctionModule
	{
		public override bool SyncValues(int[] moduleValues)
		{
            base.SyncValues(moduleValues);
            x = InputValueMapper.MapRotation(values[0]);
            y = InputValueMapper.MapRotation(values[1]);
            z = InputValueMapper.MapRotation(values[2]);
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
		float x; //values[0]
		float y; //values[1]
		float z; //values[2]
	}
}