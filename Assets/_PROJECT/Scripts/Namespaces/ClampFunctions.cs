using UnityEngine;

namespace ClampFunctions
{
    public static class ClampMethods
    {
        public static int Clamp0(this int number) => number < 0 ? 0 : number;
        public static float Clamp0(this float number) => number < 0.0f ? 0.0f : number;
        public static int Clamp(this int number, int min, int max) => Mathf.Clamp(number, min, max);
        public static float Clamp(this float number, float min, float max) => Mathf.Clamp(number, min, max);

        public static bool Is(this int number, int isNumber) => number == isNumber;
        public static bool Not(this int number, int not) => number != not;
        public static bool AtLeast(this int number, int atLeast) => number >= atLeast;
        public static bool MoreThan(this int number, int moreThan) => number > moreThan;
        public static bool AtMost(this int number, int atMost) => number <= atMost;
        public static bool LessThan(this int number, int lessThan) => number < lessThan;
    }
}