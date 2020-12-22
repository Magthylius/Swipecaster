using System.Collections.Generic;
using Type = System.Type;

namespace Create
{
    public static class A_Status
    {
        public static StatusEffect Stun(int turns = 1) => new Stun(turns, 1.0f, false, null);
        public static StatusEffect DefenceDown(int turns) => new DefenceDown(turns, 0.5f, false, 0.1f);
        public static StatusEffect Flame(int turns) => new Flame(turns, 0.35f, false, 0.05f, 0.05f);
        public static StatusEffect Poison(int turns) => new Poison(turns, 0.35f, false, 0.05f, 0.05f);
        public static StatusEffect Weakness(int turns) => new Weakness(turns, 0.65f, false, 0.2f);

        public static IEnumerable<Type> List => 
            new List<Type>()
            {
                new Stun().GetType(),
                new DefenceDown().GetType(),
                new Flame().GetType(),
                new Poison().GetType(),
                new Weakness().GetType()
            };

    }
}