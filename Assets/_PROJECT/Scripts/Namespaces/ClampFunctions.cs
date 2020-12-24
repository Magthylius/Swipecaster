using UnityEngine;

namespace ClampFunctions
{
    public static class ClampMethods
    {
        public static int Clamp0(this int number) => Mathf.Clamp(number, 0, int.MaxValue);
        public static float Clamp0(this float number) => Mathf.Clamp(number, 0.0f, float.MaxValue);
    }
}