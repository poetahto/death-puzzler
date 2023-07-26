using UnityEngine;

namespace Effects
{
    public class LightAnimation : MonoBehaviour
    {
        public Light targetLight;
        public LightAnimationStyle style;

        private float fullBrightness;

        private void Start()
        {
            fullBrightness = targetLight.intensity;
        }

        private void Update()
        {
            float intensity = style.GetIntensity();
            targetLight.intensity = Mathf.LerpUnclamped(0, fullBrightness, intensity);
        }
    }
}