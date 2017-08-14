﻿using System.Collections.Generic;
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
        public event ConnectableChangedHandler ShapeModuleCreated;

        public bool AddDataProvider(DataProvider dataProvider)
        {
            dataProvider.ModuleCommandRecieved += OnModuleCommandReceived;
            dataProviders.Add(dataProvider);
            dataProvider.Connect();
            return true;
        }
        public bool OnModuleCommandReceived(ModuleData moduleData)
        {
            Log("Module command received.");
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
                        module.SyncValues(moduleData.values);
                        //TODO: send CONNECT, LINK, etc. events (currently, every event is UPDATE)
                        if (moduleData.connectedModuleAddress == 0)
                        {
                            //TODO: unlink only the previously connected module... (currently, only mono-chains are supported)
                            module.UnlinkAll();
                            return FindModuleChains();
                        } else {
                            var debugMessage = "HUB: " + 
                               module.Function.ToString() + 
                               " module at address " + 
                               module.Address.ToString() + 
                               " linked to " + 
                               moduleData.function.ToString() +
                               " module at address " + 
                               moduleData.connectedModuleAddress.ToString() + 
                               ".";
							Log(debugMessage);
                            var upstreamModule = FindModule(moduleData.connectedModuleAddress);
                            if (upstreamModule != null)
                            {
                                module.LinkModule(upstreamModule);
                                return FindModuleChains();
                            }
                        }
                        return module.SyncValues(moduleData.values);
                    } else {
						var createdModule = ConnectModule(moduleData.address, moduleData.function);
						if (createdModule != null)
						{
							Log("HUB: " + createdModule.Function.ToString() + " module at address " + createdModule.Address.ToString() + " connected.");
							return true;
						}
                    }
                    return false;
                case Command.LINK:
                    if (module != null)
                    {
                        var upstreamModule = FindModule(moduleData.connectedModuleAddress);
                        if (upstreamModule != null)
                        {
                            return module.LinkModule(upstreamModule);
                        }
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
        public bool Operate()
        {
            foreach (var chain in moduleChains)
            {
                var shapeModule = chain.FirstOrDefault(m => m.Function == ModuleFunction.SHAPE) as Creator;
                if (shapeModule != null)
                {
                    var sceneNode = sceneController.GetSceneNode(shapeModule);
                    if (sceneNode != null)
                    {
                        foreach (var module in chain)
                        {
                            module.Operate(sceneNode);
                        }
                    }
                }
            }
            return true;
        }
        public bool Disconnect()
        {
            foreach (var dataProvider in dataProviders)
            {
                dataProvider.Disconnect();
            }
            return true;
        }
        public Hub(SceneController scene_controller)
        {
            int hubAddress = (int)ReservedAddresses.HUB;
            hubModule = new PassThrough(hubAddress);
            sceneController = scene_controller;
            dataProviders = new List<DataProvider>();
            connectedModules = new List<Connectable>();
            connectedModules.Add(hubModule);
            rootModules = new List<Connectable>();
            moduleChains = new List<List<Connectable>>();
            nodeMap = new Dictionary<ModuleLink, Transformable>();
        }
		private Connectable ConnectModule(int moduleAddress, ModuleFunction moduleFunction)
		{
			var module = ModuleProvider.CreateModule(moduleAddress, moduleFunction);
			connectedModules.Add(module);
			if (module.Function == ModuleFunction.SHAPE)
			{
                rootModules.Add(module);
                if (ShapeModuleCreated != null)
                {
                    ShapeModuleCreated(module);
                }
			}
			if (ModuleCreated != null)
			{
				ModuleCreated(module);
			}
			//TODO: Validate connection: no duplicate addresses...
			return module;
		}
        private bool FindModuleChains()
        {
            if (rootModules.Any())
            {
                moduleChains.Clear();
                foreach (var root in rootModules)
                {
                    var chain = new List<Connectable>();
                    chain.Add(root);
                    if (root.UpstreamConnections.Count > 0)
                    {
                        var nextModule = root.UpstreamConnections.FirstOrDefault();
                        while (nextModule != null)
                        {
                            chain.Add(nextModule);
                            nextModule = nextModule.UpstreamConnections.FirstOrDefault();
                        }
                    }
                    moduleChains.Add(chain);
                }
                var debugMessage = moduleChains.Count.ToString() + " module chains:\n";
                foreach (var chain in moduleChains)
                {
                    var chainMessage = "  - Chain: ";
                    foreach (var module in chain)
                    {
                        chainMessage += module.Address.ToString() + ", ";
                    }
                    debugMessage += chainMessage + "\n";
                }
                Log(debugMessage);
                return true;
            }
            return false;
        }
		private Connectable FindModule(int moduleAddress)
		{
			return connectedModules.FirstOrDefault(m => m.Address == moduleAddress);
		}
        private void Log(string message)
        {
            if (LogMessageGenerated != null)
            {
                LogMessageGenerated(message);
            }
        }
        private PassThrough hubModule;
        private SceneController sceneController;
        private List<DataProvider> dataProviders;
        private List<Connectable> connectedModules;
        private List<Connectable> rootModules;
        private List<List<Connectable>> moduleChains;
        private Dictionary<ModuleLink, Transformable> nodeMap;
    }
}