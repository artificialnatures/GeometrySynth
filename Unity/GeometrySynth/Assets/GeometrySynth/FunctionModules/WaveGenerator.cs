﻿using UnityEngine;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;
using GeometrySynth.Control;

namespace GeometrySynth.FunctionModules
{
    public class WaveGenerator : FunctionModule
    {
		public override bool SyncValues(int[] moduleValues)
		{
            base.SyncValues(moduleValues);
            amplitude = InputValueMapper.MapAmplitude(values[0]);
            frequency = InputValueMapper.MapFrequency(values[1]);
            offset = InputValueMapper.MapOffset(values[2]);
			return true;
		}
		public override bool Step(float time)
		{
            scalar = amplitude * Mathf.Sin(((Mathf.PI * 2.0f) * frequency * time) + offset);
            return true;
		}
		public override bool Operate(Transformable transformable)
		{
            transformable.Scalar = scalar;
			return true;
		}
        public WaveGenerator(int moduleAddress) : base(moduleAddress, ModuleFunction.WAVE)
		{
            amplitude = 1.0f;
            frequency = 1.0f;
            offset = 0.0f;
		}
		private float MapValue(int rawValue)
		{
			float mappedValue = (float)rawValue / 255.0f;
			return mappedValue;
		}
        private float amplitude; //values[0]
        private float frequency; //values[1]
        private float offset; //values[2]
        private float scalar;
    }
}