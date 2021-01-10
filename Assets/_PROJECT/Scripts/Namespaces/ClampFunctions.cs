using UnityEngine;

namespace ClampFunctions
{
    public static class ClampMethods
    {
        public static int Clamp0(this int number) => number < 0 ? 0 : number;
        public static float Clamp0(this float number) => number < 0.0f ? 0.0f : number;
        public static int Clamp(this int number, int min, int max) => Mathf.Clamp(number, min, max);
        public static float Clamp(this float number, float min, float max) => Mathf.Clamp(number, min, max);
    }
}