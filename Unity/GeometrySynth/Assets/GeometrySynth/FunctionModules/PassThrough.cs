using GeometrySynth.Constants;
using GeometrySynth.Interfaces;

namespace GeometrySynth.FunctionModules
{
    public class PassThrough : FunctionModule
    {
        public PassThrough(int moduleAddress)
        {
            address = moduleAddress;
            function = ModuleFunction.PASSTHROUGH;
        }
    }
}
