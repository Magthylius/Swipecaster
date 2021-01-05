using UnityEngine;

namespace Obtain
{
    public static class A_Path_For
    {
        public static string LevelConfiguration => "ScriptableObjects/Quest and Levels";
    }

    public static class A_Scriptable
    {
        public static LevelConfigurationObject LevelConfiguration
            => Resources.LoadAll<LevelConfigurationObject>(Obtain.A_Path_For.LevelConfiguration)[0];
    }
}