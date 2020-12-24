using UnityEngine;

namespace ConversionFunctions
{
    public static class GameObjectTo
    {
        public static Unit AsUnit(this GameObject gameObject) => gameObject != null ? gameObject.GetComponent<Unit>() : null;
        public static Unit AsUnit(this Transform transform) => transform != null ? transform.GetComponent<Unit>() : null;
    }
}