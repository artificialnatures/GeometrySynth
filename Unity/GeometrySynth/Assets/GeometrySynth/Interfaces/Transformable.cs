using GeometrySynth.Constants;

namespace GeometrySynth.Interfaces
{
    public interface Transformable
    {
        Creation Shape { get; set; }
        float Scalar { get; set; }
        bool IsArrayed { get; }
        bool Array(int countX, int countY, int countZ, float spacingX, float spacingY, float spacingZ);
        bool Translate(float x, float y, float z);
        bool Rotate(float x, float y, float z);
        bool Scale(float x, float y, float z);
        bool Color(float r, float g, float b);
    }
}