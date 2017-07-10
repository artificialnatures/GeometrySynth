﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GeometrySynth.Constants;
using GeometrySynth.Control;
using GeometrySynth.Interfaces;

namespace GeometrySynth.UI
{
    public class ScreenControls : MonoBehaviour, DataProvider
    {
        public GameObject canvas;
        public Transform scrollViewContent;
        public GameObject modulePanelPrefab;
		public Dropdown upstreamDropdown;
		public Dropdown downstreamDropdown;

        public event ModuleDataChangedHandler ModuleCommandRecieved;

        public void AddModulePanel(Connectable module)
        {
            var modulePanel = Instantiate(modulePanelPrefab) as GameObject;
            var modulePanelTransform = modulePanel.GetComponent<RectTransform>();
            var modulePanelController = modulePanel.GetComponent<ModulePanelController>();
            modulePanelTransform.SetParent(scrollViewContent);
            modulePanelTransform.localPosition = new Vector3(
                (modulePanelTransform.rect.width * 0.5f) + (modulePanelTransform.rect.width * (float)modulePanels.Count),
                (-modulePanelTransform.rect.height * 0.5f),
                modulePanelTransform.localPosition.z
                );
            modulePanelController.SetModule(module);
            modulePanels.Add(modulePanelController);
        }
        public void RemoveModulePanel(Connectable module)
        {
            
        }

		public bool Connect()
		{
            return false;
		}
		public bool Disconnect()
		{
            return false;
		}
		public bool RequestUpdate(int address)
		{
            return false;
		}
		public bool Receive()
        {
            return false;
        }
		public bool Send(ModuleData moduleData)
        {
            return false;
        }

		public void CreateShapeModule()
		{
            CreateFakeModule(ModuleFunction.CREATE);
		}
		public void CreateTranslateModule()
		{
            CreateFakeModule(ModuleFunction.TRANSLATE);
		}
		public void CreateRotateModule()
		{
            CreateFakeModule(ModuleFunction.ROTATE);
		}
		public void CreateScaleModule()
		{
            CreateFakeModule(ModuleFunction.SCALE);
		}
		public void CreateColorModule()
		{
            CreateFakeModule(ModuleFunction.COLOR);
		}
		public void CreateWaveModule()
		{
            CreateFakeModule(ModuleFunction.SINE_WAVE);
		}
		public void CreatePulseModule()
		{
            CreateFakeModule(ModuleFunction.PULSE);
		}

		public void SetUpstreamAddress(int address)
		{
			upstreamAddress = address;
		}
		public void SetDownstreamAddress(int address)
		{
			downstreamAddress = address;
		}
		public void LinkModules()
		{
            if (ModuleCommandRecieved != null)
            {
                var moduleCommand = new ModuleData()
                {
                    address = downstreamAddress,
                    command = Command.LINK,
                    connectedModuleAddress = upstreamAddress
                };
                ModuleCommandRecieved(moduleCommand);
            }
		}

        void Start()
        {
            modulePanels = new List<ModulePanelController>();
            newModuleAddress = 1024;
            PopulateAddressDropdowns();
        }
        void Update()
        {
            ProcessKeyCommands();
        }

        private void CreateFakeModule(ModuleFunction moduleFunction)
        {
			if (ModuleCommandRecieved != null)
			{
				newModuleAddress++;
				var moduleCommand = new ModuleData();
				moduleCommand.address = newModuleAddress;
				moduleCommand.command = Command.CONNECT;
				moduleCommand.function = moduleFunction;
				ModuleCommandRecieved(moduleCommand);
			}
        }
        private void ProcessKeyCommands()
        {
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                if (Input.GetKeyUp(KeyCode.V))
                {
                    ToggleVisibility();
                }
            }
        }
        private void ToggleVisibility()
        {
            if (canvas.activeSelf)
            {
                canvas.SetActive(false);
            }
            else
            {
                canvas.SetActive(true);
            }
        }
		private void PopulateAddressDropdowns()
		{
			upstreamDropdown.ClearOptions();
			downstreamDropdown.ClearOptions();
			var upstreamOptions = new List<Dropdown.OptionData>();
			var downstreamOptions = new List<Dropdown.OptionData>();
			for (int i = 0; i < 100; i++)
			{
				upstreamOptions.Add(new Dropdown.OptionData(i.ToString()));
				downstreamOptions.Add(new Dropdown.OptionData(i.ToString()));
			}
			upstreamDropdown.AddOptions(upstreamOptions);
			downstreamDropdown.AddOptions(downstreamOptions);
		}

        private List<ModulePanelController> modulePanels;
        private int newModuleAddress;
		private int upstreamAddress;
		private int downstreamAddress;
    }
}