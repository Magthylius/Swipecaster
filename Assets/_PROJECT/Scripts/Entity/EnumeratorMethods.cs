using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

public static class EnumeratorMethods
{
    public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorParamaters) where T : class
    {
        IList<T> enumerables = new List<T>();
        var types = Assembly.GetAssembly(typeof(T)).GetTypes().Where(type => type.IsClass && type.IsAbstract && type.IsSubclassOf(typeof(T)));

        int i = 0;
        while(i < types.Count())
        {
            var type = types.ElementAt(i);
            enumerables.Add((T)Activator.CreateInstance(type, constructorParamaters));
            i++;
        }
        return enumerables;
    }

    static EnumeratorMethods() { }
}