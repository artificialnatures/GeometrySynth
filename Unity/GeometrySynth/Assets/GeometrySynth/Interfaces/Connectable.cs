using System.Collections.Generic;

using GeometrySynth.Constants;

namespace GeometrySynth.Interfaces
{
    public interface Connectable
    {
        event ConnectableChangedHandler ModuleDataChanged;
        int Address { get; }
        ModuleFunction Function { get; }
        List<Connectable> UpstreamConnections { get; }
        List<Connectable> DownstreamConnections { get; }
        bool LinkModule(Connectable upstreamModule);
        bool UnlinkModule(Connectable upstreamModule);
        bool UnlinkAll();
        bool AddDownstreamConnection(Connectable downstreamModule);
        bool RemoveDownstreamConnection(Connectable downstreamModule);
        int InputCount { get; }
        int[] InputValues { get; }
        bool SyncValues(int[] values);
        bool Step(float time);
        bool Operate(Transformable transformable);
    }
}