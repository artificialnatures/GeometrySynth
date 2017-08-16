namespace GeometrySynth.Constants
{
    public enum ModuleFunction
    {
        PASSTHROUGH = 2,
        //Creation:
        SHAPE = 3,
        ARRAY = 4,
        //Transformation:
        TRANSLATE = 10,
        ROTATE = 11,
        SCALE = 12,
        MATRIX = 13,
        //Multipliers:
        WAVE = 20,
        SINE_WAVE = 21,
        SQUARE_WAVE = 22,
        TRIANGLE_WAVE = 23,
        PULSE = 24,
        NOISE = 25,
        NOISE_PERLIN = 26,
        //Material:
        COLOR = 30,
        COLOR_ALPHA = 31,
        TEXTURE = 32,
        BUMP = 33,
        //Lights:
        LIGHT = 40,
        LIGHT_DIRECTIONAL = 41,
        LIGHT_SPOT = 42,
        LIGHT_POINT = 43,
        //Camera:
        CAMERA_POSITION = 50,
        CAMERA_ORBIT = 51,
        CAMERA_PAN = 52,
        CAMERA_FOV = 53,
        //Filters:
        FILTER_LOWPASS = 60,
        FILTER_HIGHPASS = 61,
        //Triggers:
        TOGGLE = 70,
        BLINK = 71
    };
}