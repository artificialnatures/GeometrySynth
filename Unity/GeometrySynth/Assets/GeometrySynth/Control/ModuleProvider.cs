using System.Collections.Generic;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;
using GeometrySynth.FunctionModules;

namespace GeometrySynth.Control
{
    public static class ModuleProvider
    {
        public static Connectable CreateModule(int moduleAddress, ModuleFunction moduleFunction)
        {
            switch(moduleFunction)
            {
                case ModuleFunction.SHAPE:
                    return new Creator(moduleAddress);
                case ModuleFunction.TRANSLATE:
                    return new Translator(moduleAddress);
                case ModuleFunction.ROTATE:
                    return new Rotator(moduleAddress);
                case ModuleFunction.SCALE:
                    return new Scaler(moduleAddress);
                case ModuleFunction.COLOR:
                    return new Colorer(moduleAddress);
                case ModuleFunction.SINE_WAVE:
                    return new WaveGenerator(moduleAddress);
            }
            return null;
        }
    }
}