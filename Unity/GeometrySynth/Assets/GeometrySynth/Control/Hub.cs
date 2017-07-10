using System.Collections.Generic;
using System.Linq;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;
using GeometrySynth.FunctionModules;

namespace GeometrySynth.Control
{
    public class Hub
    {
        public event StringValueChangedHandler LogMessageGenerated;
        public event ConnectableChangedHandler ModuleCreated;

        public bool AddDataProvider(DataProvider dataProvider)
        {
            dataProvider.ModuleCommandRecieved += OnModuleCommandReceived;
            dataProviders.Add(dataProvider);
            dataProvider.Connect();
            return true;
        }
        public Connectable ConnectModule(int moduleAddress, ModuleFunction moduleFunction)
        {
            var module = ModuleProvider.CreateModule(moduleAddress, moduleFunction);
            connectedModules.Add(module);
            if (ModuleCreated != null)
            {
                ModuleCreated(module);
            }
            //TODO: Validate connection: no duplicate addresses...
            return module;
        }
        public bool LinkModules(int upstreamModuleAddress, int downstreamModuleAddress)
        {
            var upstreamModule = FindModule(upstreamModuleAddress);
            var downstreamModule = FindModule(downstreamModuleAddress);
            if (upstreamModule != null && downstreamModule != null)
            {
                //TODO: Validate link...
                var moduleLink = new ModuleLink(upstreamModule, downstreamModule);
                moduleLinks.Add(moduleLink);
                return true;
            } else {
                return false;
            }
        }
        public Connectable FindModule(int moduleAddress)
        {
            return connectedModules.FirstOrDefault(m => m.Address == moduleAddress);
        }
        public bool OnModuleCommandReceived(ModuleData moduleData)
        {
            var module = FindModule(moduleData.address);
            switch(moduleData.command)
            {
                case Command.CONNECT:
                    var newModule = ConnectModule(moduleData.address, moduleData.function);
                    if (newModule != null) 
                    {
                        Log("HUB: " + newModule.Function.ToString() + " module at address " + newModule.Address.ToString() + " connected.");
                        return true;
                    }
                    return false;
                case Command.UPDATE:
                    if (module != null)
                    {
                        Log("HUB: " + module.Function.ToString() + " module at address " + module.Address.ToString() + " sent update.");
                        return module.SyncValues(moduleData.values);
                    }
                    return false;
                case Command.LINK:
                    if (module != null)
                    {
                        Log("HUB: " + module.Function.ToString() + " module at address " + module.Address.ToString() + " linked to module at address " + moduleData.connectedModuleAddress.ToString() + ".");
                        return LinkModules(moduleData.connectedModuleAddress, moduleData.address);
                    }
                    return false;
            }
            return false;
        }
        public bool Sync()
        {
            foreach (var dataProvider in dataProviders)
            {
                dataProvider.Receive();
            }
            return true;
        }
        public bool Step(float time)
        {
            foreach (var module in connectedModules)
            {
                module.Step(time);
            }
            return true;
        }
        public bool Operate(Transformable node)
        {
            foreach (var module in connectedModules)
            {
                module.Operate(node);
            }
            return true;
        }
        public Hub()
        {
            int hubAddress = (int)ReservedAddresses.HUB;
            hubModule = new PassThrough(hubAddress);
            dataProviders = new List<DataProvider>();
            connectedModules = new List<Connectable>();
            connectedModules.Add(hubModule);
            moduleLinks = new List<ModuleLink>();
            sceneNodes = new List<Transformable>();
            nodeMap = new Dictionary<ModuleLink, Transformable>();
        }
        private void Log(string message)
        {
            if (LogMessageGenerated != null)
            {
                LogMessageGenerated(message);
            }
        }
        private PassThrough hubModule;
        private List<DataProvider> dataProviders;
        private List<Connectable> connectedModules;
        private List<ModuleLink> moduleLinks;
        private List<Transformable> sceneNodes;
        private Dictionary<ModuleLink, Transformable> nodeMap;
    }
}