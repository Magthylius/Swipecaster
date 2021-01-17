using ClampFunctions;
using System.Collections.Generic;
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

    public static class ArrayGetter
    {
        public static T Random<T>(this T[] tArray) => tArray[UnityEngine.Random.Range(0, tArray.Length)];
        public static T Random<T>(this List<T> tList) => tList[UnityEngine.Random.Range(0, tList.Count)];

        public static bool ValidIndex<T>(this T[] tArray, int index) => index.AtLeast(0) && index.LessThan(tArray.Length);
        public static bool ValidIndex<T>(this List<T> tList, int index) => index.AtLeast(0) && index.LessThan(tList.Count);
    }
}