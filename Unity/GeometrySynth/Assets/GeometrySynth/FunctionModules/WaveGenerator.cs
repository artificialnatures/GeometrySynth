﻿using UnityEngine;
using System.Collections.Generic;

using GeometrySynth.Constants;
using GeometrySynth.Interfaces;

namespace GeometrySynth.FunctionModules
{
    public class WaveGenerator : FunctionModule
    {
		public override bool SyncValues(int[] moduleValues)
		{
            if (moduleValues.Length == 3)
			{
                values = moduleValues;
                amplitude = MapValue(values[0]);
                frequency = MapValue(values[1]);
                offset = MapValue(values[2]);
				return true;
			} else {
				return false;
			}
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
        public WaveGenerator(int moduleAddress)
		{
			address = moduleAddress;
            function = ModuleFunction.SINE_WAVE;
            values = new int[] { 255, 255, 0 };
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