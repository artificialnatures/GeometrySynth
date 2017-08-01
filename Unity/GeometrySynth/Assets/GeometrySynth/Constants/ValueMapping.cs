namespace GeometrySynth.Constants
{
    public enum ValueMapping
    {
        NONE, //passthrough: 0 to 255
        FLOAT_NORMALIZED, //-1.0f to 1.0f
        FLOAT_POSITIVE, //0.0f to 1.0f
        INTEGER, //0 to 255
        BOOLEAN //true (>0) or false (0)
    }
}