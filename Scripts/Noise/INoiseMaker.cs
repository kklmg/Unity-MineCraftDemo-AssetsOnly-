using UnityEngine;

namespace Assets.Scripts.Noise
{
    public interface INoiseMaker
    {
        float GetNoise_2D(Vector2 point);
        float GetNoise_2D(Vector2 point, float frequency, float amplitude);

        float GetOctaveNoise_2D(Vector2 point, float frequency, float amplitude,int octave= 8);


        float GetNoise_3D(Vector3 point);
    }
}
