namespace GeometrySynth.Interfaces
{
    public delegate bool StringValueChangedHandler(string stringValue);
    public delegate bool BoolValueChangedHandler(bool boolValue);
    public delegate bool IntValueChangedHandler(int intValue);
    public delegate bool IntPairValueChangedHandler(int value1, int value2);
    public delegate bool IntArrayValueChangedHandler(int[] intArray);
    public delegate bool FloatValueChangedHandler(float floatValue);
    public delegate bool FloatArrayValueChangedHandler(float[] floatArray);
    public delegate bool ModuleDataChangedHandler(ModuleData moduleData);
    public delegate bool ConnectableChangedHandler(Connectable connectable);
    public delegate bool MomentaryEventHandler();
}
