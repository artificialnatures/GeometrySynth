using GeometrySynth.Constants;

namespace GeometrySynth.Interfaces
{
    public interface Transformable
    {
        Connectable Module { get; }
        bool IsActive { get; set; }
        Shape Shape { get; set; }
        float Scalar { get; set; }
        bool IsArrayed { get; set; }
        bool Array(int countX, int countY, int countZ);
        bool Translate(float x, float y, float z);
        bool Rotate(float x, float y, float z);
        bool Scale(float x, float y, float z);
        bool ApplyColor(float r, float g, float b);
    }
}