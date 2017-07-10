using GeometrySynth.Constants;

namespace GeometrySynth.Interfaces
{
    [System.Serializable]
    public struct ModuleData
    {
        public int address;
        public ModuleFunction function;
        public Command command;
        public int[] values;
        public int connectedModuleAddress;
    }
}
