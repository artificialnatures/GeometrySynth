using System.Collections.Generic;

using GeometrySynth.Constants;

namespace GeometrySynth.Interfaces
{
    public interface Connectable
    {
        event ModuleDataChangedHandler ModuleDataChanged;
        int Address { get; }
        ModuleFunction Function { get; }
        int InputCount { get; }
        int[] InputValues { get; }
        List<Connectable> UpstreamConnections { get; }
        List<Connectable> DownstreamConnections { get; }
        bool ConnectToUpstreamModule(Connectable connectable);
        bool ConnectToDownstreamModule(Connectable connectable);
        bool SyncValues(int[] values);
        bool Step(float time);
        bool Operate(Transformable transformable);
    }
}