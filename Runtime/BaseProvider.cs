using UnityEngine;

namespace Noise
{
    public abstract class BaseProvider : ScriptableObject
    {
        public int seed;
       
        [Header("Preview")] [SerializeField] protected bool preview;
        [SerializeField] protected int previewSize;
        [SerializeField] protected Gradient colorGradient;
        [SerializeField] public Texture2D previewTexture;
        public abstract void Initialize(int newSeed);
        public abstract float GetValue(Vector3 pos);
        
        protected virtual void OnValidate()
        {
            if (!preview) return;
            Initialize(seed);
            previewSize = Mathf.Max(previewSize, 1);
            if (!previewTexture)
            {
                previewTexture = new Texture2D(previewSize, previewSize);
            }

            previewTexture.Reinitialize(previewSize, previewSize);
            var min = float.MaxValue;
            var max = float.MinValue;
            var data = new float[previewSize, previewSize];
            for (var i = 0; i < previewSize; i++)
            for (var j = 0; j < previewSize; j++)
            {
                var v = GetValue(new Vector3(i*1f/previewSize, j*1f/previewSize));
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