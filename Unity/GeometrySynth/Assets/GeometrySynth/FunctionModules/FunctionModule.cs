using System.Collections.Generic;
using System.Linq;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;

namespace GeometrySynth.FunctionModules
{
    public class FunctionModule : Connectable
    {
        public event ModuleDataChangedHandler ModuleDataChanged;
        public ModuleData Data
        {
            get
            {
                var data = new ModuleData();
                data.address = address;
                data.function = function;
                data.values = values;
                return data;
            }
        }
        public int Address
        {
            get { return address; }
        }
        public ModuleFunction Function
        {
            get { return function; }
        }
        public int InputCount
        {
            get { return values.Length; }
        }
        public int[] InputValues
        {
            get { return values; }
        }
        public List<Connectable> UpstreamConnections
        {
            get { return upstreamConnections; }
        }
        public List<Connectable> DownstreamConnections
        {
            get { return downstreamConnections; }
        }
        public bool ConnectToUpstreamModule(Connectable connectable)
        {
            if (!upstreamConnections.Any(c => c.Address == connectable.Address))
            {
                upstreamConnections.Add(connectable);
                connectable.ConnectToDownstreamModule(this);
                return true;
            }
            return false;
        }
        public bool ConnectToDownstreamModule(Connectable connectable)
        {
            if (!downstreamConnections.Any(c => c.Address == connectable.Address))
            {
                downstreamConnections.Add(connectable);
                return true;
            }
            return false;
        }
        public virtual bool SyncValues(int[] moduleValues)
        {
            values = moduleValues;
            return false;
        }
        public virtual bool Step(float time)
        {
            return false;
        }
        public virtual bool Operate(Transformable transformable)
        {
            return false;
        }
        public FunctionModule()
        {
            address = 0;
            function = ModuleFunction.PASSTHROUGH;
            values = new int[] { };
        }
        public FunctionModule(int moduleAddress, ModuleFunction moduleFunction)
        {
            address = moduleAddress;
            function = moduleFunction;
            values = new int[] { };
        }
        protected int address;
        protected ModuleFunction function;
        protected List<Connectable> upstreamConnections;
        protected List<Connectable> downstreamConnections;
        protected int[] values;
    }
}
