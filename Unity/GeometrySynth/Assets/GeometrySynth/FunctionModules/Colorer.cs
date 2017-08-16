using System.Collections.Generic;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;
using GeometrySynth.Control;

namespace GeometrySynth.FunctionModules
{
	public class Colorer : FunctionModule
	{
		public override bool SyncValues(int[] moduleValues)
		{
            base.SyncValues(moduleValues);
            r = InputValueMapper.MapColor(values[0]);
            g = InputValueMapper.MapColor(values[1]);
            b = InputValueMapper.MapColor(values[2]);
			return true;
		}
		public override bool Step(float time)
		{
			return false;
		}
		public override bool Operate(Transformable transformable)
		{
			transformable.ApplyColor(r, g, b);
			return true;
		}
        public Colorer(int moduleAddress) : base(moduleAddress, ModuleFunction.COLOR)
		{
			r = 1.0f;
			g = 1.0f;
			b = 1.0f;
		}
		float r; //values[0]
		float g; //values[1]
		float b; //values[2]
	}
}