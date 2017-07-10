using GeometrySynth.Interfaces;

namespace GeometrySynth.Control
{
    public class ModuleLink
    {
        public Connectable UpstreamModule
        {
            get { return upstreamModule; }
        }
        public Connectable DownstreamModule
        {
            get { return downstreamModule; }
        }
        public ModuleLink(Connectable upstream, Connectable downstream)
        {
            upstreamModule = upstream;
            downstreamModule = downstream;
        }
        private Connectable upstreamModule;
        private Connectable downstreamModule;
    }
}
