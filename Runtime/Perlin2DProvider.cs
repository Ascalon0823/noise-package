using UnityEngine;

namespace Noise
{
    [CreateAssetMenu(fileName = "New Perlin2D Noise", menuName = "Noise/Perlin2D", order = 0)]
    public class Perlin2DProvider : BaseProvider
    {
        [Range(1, 10)] public int octave;
        [Range(0.01f, 1f)] public float frequency;
        [Range(0f, 1f)] public float persistence;
        [Range(1f, 10f)] public float lacunarity;
        [SerializeField] private Vector2 offset;

        public override float[,] GetData(int x, int y, int newSeed, float?[,] baseData = null)
        {
            Random.InitState(seed);
            offset = Random.insideUnitCircle * 1000f;
            var data = new float[x, y];
            for (var i = 0; i < x; i++)
            for (var j = 0; j < y; j++)
            {
                data[i, j] = GetValue(new Vector3(i * 1f / x, j * 1f / y));
            }
            return data;
        }

        private float GetValue(Vector3 pos)
        {
            
            var x = pos.x;
            var y = pos.y;
            var value = 0f;
            var amplitude = 1f;
            var maxAmp = 0f;
            var f = frequency;
            for (var i = 0; i < octave; i++)
            {
                maxAmp += amplitude;
                value += amplitude * Mathf.PerlinNoise(x / f + offset.x, y / f + offset.y);
                amplitude *= persistence;
                f /= lacunarity;
            }

            return value/maxAmp;
        }
    }
}