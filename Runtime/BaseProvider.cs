using UnityEngine;

namespace Noise
{
    public abstract class BaseProvider : ScriptableObject
    {
        [Header("Preview")]
        public int seed;
        [SerializeField] protected bool preview;
        [SerializeField] protected int previewSize;
        [SerializeField] protected Gradient colorGradient;
        [SerializeField] public Texture2D previewTexture;
        
        public abstract float[,] GetData(int sizeX, int sizeY, int newSeed, float?[,] baseData = null);
        protected virtual void OnValidate()
        {
            if (!preview) return;
            previewSize = Mathf.Max(previewSize, 1);
            if (!previewTexture)
            {
                previewTexture = new Texture2D(previewSize, previewSize);
            }

            previewTexture.Reinitialize(previewSize, previewSize);
            var min = float.MaxValue;
            var max = float.MinValue;
            var data = GetData(previewSize, previewSize, seed);
            for (var i = 0; i < previewSize; i++)
            for (var j = 0; j < previewSize; j++)
            {
                var v = data[i, j];
                if (v > max) max = v;
                if (v < min) min = v;
                data[i, j] = v;
            }

            for (var i = 0; i < previewSize; i++)
            for (var j = 0; j < previewSize; j++)
            {
                previewTexture.SetPixel(i, j, colorGradient.Evaluate(Mathf.InverseLerp(min, max, data[i, j])));
            }

            previewTexture.Apply();
        }
    }
}