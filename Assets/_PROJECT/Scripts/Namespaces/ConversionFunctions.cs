using UnityEngine;

namespace ConversionFunctions
{
    public static class GameObjectTo
    {
        public static Unit AsUnit(this GameObject gameObject) => gameObject.GetComponent<Unit>();
    }
}