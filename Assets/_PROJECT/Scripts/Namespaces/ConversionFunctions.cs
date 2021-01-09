using UnityEngine;

namespace ConversionFunctions
{
    public static class GameObjectTo
    {
        public static Unit AsUnit(this GameObject gameObject) => gameObject != null ? gameObject.GetComponent<Unit>() : null;
        public static Unit AsUnit<T>(this T tee) where T : Component => tee != null ? tee.GetComponent<Unit>() : null;
        public static Summon AsSummon(this GameObject gameObject) => gameObject != null ? gameObject.GetComponent<Summon>() : null;
        public static Summon AsSummon<T>(this T tee) where T : Component => tee != null ? tee.GetComponent<Summon>() : null;

        public static T AsType<T>(this GameObject gameObject) where T : Component => gameObject != null ? gameObject.GetComponent<T>() : null;
        public static T AsType<T>(this T tee) where T : Component => tee != null ? tee.GetComponent<T>() : null;
    }
}