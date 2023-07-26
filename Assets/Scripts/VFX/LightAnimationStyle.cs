using UnityEngine;

namespace Effects
{
    [CreateAssetMenu]
    public class LightAnimationStyle : ScriptableObject
    {
        [Tooltip("a = Total Darkness\nm = Full Brightness\nz = Double Brightness")]
        [SerializeField] private string animationCode;

        [Tooltip("How many characters to process per second.")]
        [SerializeField] private float animationSpeed = 10f;

        [Tooltip("Should changing values be smoothed out?")]
        [SerializeField] private bool interpolateIntensity;
        
        public float GetIntensity()
        {
            float time = Time.time * animationSpeed;
            int roundedTime = (int) time;
            int currentIndex = roundedTime % animationCode.Length;
            float currentIntensity = CalculateIntensity(currentIndex);

            if (interpolateIntensity)
            {
                int nextIndex = (currentIndex + 1) % animationCode.Length;
                float nextIntensity = CalculateIntensity(nextIndex); 
                float t = time - roundedTime;
                return Mathf.Lerp(currentIntensity, nextIntensity, t);
            }
            
            return currentIntensity;
        }

        private float CalculateIntensity(int index)
        {
            return (animationCode[index] - 'a') / 13f;
        }
    }
}