using UnityEngine;

namespace GeometrySynth.Control
{
    public static class InputValueMapper
    {
        public static readonly int MIN_INPUT_VALUE = 0;
        public static readonly int MAX_INPUT_VALUE = 255;
        public static readonly int HALF_INPUT_VALUE = 128;
        public static readonly int INPUT_INTERVAL = 10;
        public static readonly float MIN_TRANSLATION = -5.0f;
        public static readonly float MAX_TRANSLATION = 5.0f;
		public static readonly float MIN_ROTATION = -180.0f;
		public static readonly float MAX_ROTATION = 180.0f;
		public static readonly float MIN_SCALE = 1.0f;
		public static readonly float MAX_SCALE = 5.0f;
		public static readonly float MIN_COLOR = 0.0f;
		public static readonly float MAX_COLOR = 1.0f;
        public static readonly float MIN_AMPLITUDE = 0.0f;
        public static readonly float MAX_AMPLITUDE = 1.0f;
        public static readonly float MIN_FREQUENCY = 0.0f;
        public static readonly float MAX_FREQUENCY = 10.0f;
		public static readonly float MIN_OFFSET = 0.0f;
		public static readonly float MAX_OFFSET = 5.0f;

        public static bool MapBoolean(int inputValue)
        {
            if (inputValue > HALF_INPUT_VALUE)
            {
                return true;
            }
            return false;
        }
        public static float MapTranslation(int inputValue)
        {
            var outputValue = 0.0f;
            if (inputValue <= 128)
            {
                outputValue = (MIN_TRANSLATION * (float)(HALF_INPUT_VALUE - inputValue)) / (float)MAX_INPUT_VALUE;
            } else {
                outputValue = (MAX_TRANSLATION * (float)(inputValue - HALF_INPUT_VALUE)) / (float)MAX_INPUT_VALUE;
            }
            return outputValue;
        }
		public static float MapRotation(int inputValue)
		{
			var outputValue = 0.0f;
			if (inputValue <= 128)
			{
				outputValue = (MIN_ROTATION * (float)(HALF_INPUT_VALUE - inputValue)) / (float)MAX_INPUT_VALUE;
			}
			else
			{
                outputValue = (MAX_ROTATION * (float)(inputValue - HALF_INPUT_VALUE)) / (float)MAX_INPUT_VALUE;
			}
			return outputValue;
		}
		public static float MapScale(int inputValue)
		{
			var outputValue = 0.0f;
            outputValue = ((MAX_SCALE - MIN_SCALE) * (float)inputValue) / (float)MAX_INPUT_VALUE;
			return outputValue;
		}
        public static float MapColor(int inputValue)
        {
            var outputValue = 0.0f;
            outputValue = ((MAX_COLOR - MIN_COLOR) * (float)inputValue) / (float)MAX_INPUT_VALUE;
            return outputValue;
        }
		public static float MapAmplitude(int inputValue)
		{
			var outputValue = 0.0f;
            outputValue = ((MAX_AMPLITUDE - MIN_AMPLITUDE) * (float)inputValue) / (float)MAX_INPUT_VALUE;
			return outputValue;
		}
        public static float MapFrequency(int inputValue)
		{
			var outputValue = 0.0f;
            outputValue = ((MAX_FREQUENCY - MIN_FREQUENCY) * (float)inputValue) / (float)MAX_INPUT_VALUE;
			return outputValue;
		}
		public static float MapOffset(int inputValue)
		{
			var outputValue = 0.0f;
            outputValue = ((MAX_OFFSET - MIN_OFFSET) * (float)inputValue) / (float)MAX_INPUT_VALUE;
			return outputValue;
		}
        public static int MapIntegerChoice(int inputValue, int minValue, int maxValue)
        {
            var inputDivisions = maxValue - minValue;
            var mappedInterval = MAX_INPUT_VALUE / inputDivisions;
            var intervalStart = 0;
            var intervalEnd = mappedInterval;
            for (int i = 0; i < inputDivisions; i++)
            {
                intervalStart = i * mappedInterval;
                if (i < INPUT_INTERVAL - 1)
                {
                    intervalEnd = (i + 1) * mappedInterval;
                } else {
                    intervalEnd = MAX_INPUT_VALUE;
                }
                if (inputValue >= intervalStart && inputValue < intervalEnd)
                {
                    return i;
                }
            }
            return 1;
        }
    }
}