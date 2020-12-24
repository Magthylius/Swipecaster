using UnityEngine;

namespace ConversionFunctions
{
    public static class GameObjectTo
    {
        public static Unit AsUnit<T>(this T item) where T : Component => item != null ? item.GetComponent<Unit>() : null;
    }
}