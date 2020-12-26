using UnityEngine;

namespace ClampFunctions
{
    public static class ClampMethods
    {
        public static int Clamp0(this int number) => Mathf.Clamp(number, 0, int.MaxValue);
        public static float Clamp0(this float number) => Mathf.Clamp(number, 0.0f, float.MaxValue);
        public static int Clamp(this int number, int min, int max) => Mathf.Clamp(number, min, max);
        public static float Clamp(this float number, float min, float max) => Mathf.Clamp(number, min, max);
    }
}