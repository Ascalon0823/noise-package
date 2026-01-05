using UnityEditor;
using UnityEngine;
namespace Noise.Editor
{
    [CustomPreview(typeof(BaseProvider))]
    public class NoiseProviderPreview : ObjectPreview
    {
        public override bool HasPreviewGUI()
        {
            return true;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            var provider = target as BaseProvider;
            if (provider)
                GUI.DrawTexture(r,provider.previewTexture);
            else base.OnPreviewGUI(r, background);
        }
    }
}