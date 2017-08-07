using GeometrySynth.Constants;

namespace GeometrySynth.Interfaces
{
	[System.Serializable]
	public struct ModuleRequest
	{
		public Command command;
        public int[] values;
	}
}