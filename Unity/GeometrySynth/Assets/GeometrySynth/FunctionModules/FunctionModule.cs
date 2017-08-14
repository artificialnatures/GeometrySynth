using System;
using System.Collections.Generic;
using System.Linq;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;

namespace GeometrySynth.FunctionModules
{
    public class FunctionModule : Connectable
    {
        public event ConnectableChangedHandler ModuleDataChanged;
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
        public List<Connectable> UpstreamConnections
        {
            get { return upstreamConnections; }
        }
        public List<Connectable> DownstreamConnections
        {
            get { return downstreamConnections; }
        }
        public int InputCount
        {
            get { return values.Length; }
        }
        public int[] InputValues
        {
            get { return values; }
        }
        public bool LinkModule(Connectable upstreamModule)
        {
            if (!upstreamConnections.Contains(upstreamModule))
            {
                upstreamConnections.Add(upstreamModule);
                upstreamModule.AddDownstreamConnection(this);
                if (ModuleDataChanged != null) ModuleDataChanged(this);
                return true;
            }
            return false;
        }
		public bool UnlinkModule(Connectable upstreamModule)
        {
            if (upstreamConnections.Contains(upstreamModule))
            {
                upstreamConnections.Remove(upstreamModule);
                upstreamModule.RemoveDownstreamConnection(this);
                if (ModuleDataChanged != null) ModuleDataChanged(this);
                return true;
            }
            return false;
        }
        public bool UnlinkAll()
        {
            foreach (var module in upstreamConnections)
            {
                module.RemoveDownstreamConnection(this);
            }
            upstreamConnections.Clear();
            if (ModuleDataChanged != null) ModuleDataChanged(this);
            return true;
        }
		public bool AddDownstreamConnection(Connectable downstreamModule)
        {
            if (!downstreamConnections.Contains(downstreamModule))
            {
                downstreamConnections.Add(downstreamModule);
                if (ModuleDataChanged != null) ModuleDataChanged(this);
                return true;
            }
            return false;
        }
		public bool RemoveDownstreamConnection(Connectable downstreamModule)
		{
			if (downstreamConnections.Contains(downstreamModule))
			{
				downstreamConnections.Remove(downstreamModule);
                if (ModuleDataChanged != null) ModuleDataChanged(this);
				return true;
			}
			return false;
		}
        public virtual bool SyncValues(int[] moduleValues)
        {
            var valuesChanged = false;
            for (int i = 0; i < moduleValues.Length; i++)
            {
                if (i < values.Length)
                {
                    if (values[i] != moduleValues[i])
                    {
                        values[i] = moduleValues[i];
                        valuesChanged = true;
                    }
                }
            }
            if (valuesChanged)
            {
                if (ModuleDataChanged != null) ModuleDataChanged(this);
            }
            return valuesChanged;
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
            upstreamConnections = new List<Connectable>();
            downstreamConnections = new List<Connectable>();
            values = new int[] { 0, 0, 0, 0 };
        }
        public FunctionModule(int moduleAddress, ModuleFunction moduleFunction) : this()
        {
            address = moduleAddress;
            function = moduleFunction;
        }
        protected int address;
        protected ModuleFunction function;
        protected List<Connectable> upstreamConnections;
        protected List<Connectable> downstreamConnections;
        protected int[] values;
    }
}