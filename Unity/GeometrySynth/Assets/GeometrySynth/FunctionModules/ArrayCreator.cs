using System.Collections.Generic;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;
using GeometrySynth.Control;

namespace GeometrySynth.FunctionModules
{
	public class ArrayCreator : FunctionModule
	{
		public override bool SyncValues(int[] moduleValues)
		{
			base.SyncValues(moduleValues);
            x = InputValueMapper.MapIntegerChoice(values[0], 1, 10);
            y = InputValueMapper.MapIntegerChoice(values[1], 1, 10);
            z = InputValueMapper.MapIntegerChoice(values[2], 1, 10);
			return true;
		}
		public override bool Operate(Transformable transformable)
		{
			transformable.Array(x, y, z);
			return true;
		}
        public ArrayCreator(int moduleAddress) : base(moduleAddress, ModuleFunction.SCALE)
		{
			x = 1;
			y = 1;
			z = 1;
		}
		int x; //values[0]
		int y; //values[1]
		int z; //values[2]
	}
}