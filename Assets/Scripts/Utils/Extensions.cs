using UnityEngine;


namespace Utils
{
    public static class Extensions
    {
        public static void Clamp0360(this ref float eulerAngles)
        {
            float result = eulerAngles - Mathf.CeilToInt(eulerAngles / 360f) * 360f;
            if (result < 0)
            {
                result += 360f;
            }
            eulerAngles = result;
        }

        public static void ClampRef(this ref float eulerAngles, float minValue, float maxValue)
        {
            if (eulerAngles < minValue) eulerAngles = minValue;
            if (eulerAngles > maxValue) eulerAngles = maxValue;
        }
    }
}


