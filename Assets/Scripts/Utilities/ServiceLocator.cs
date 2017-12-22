using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public sealed class ServiceLocator
{
    private static readonly ServiceLocator instance = new ServiceLocator();

    private static Dictionary<string, object> dict = new Dictionary<string, object>();

    private ServiceLocator() { }

    public static ServiceLocator Instance
    {
        get
        {
            return instance;
        }
    }

    public static void Reset()
    {
        dict.Clear();
    }

    public static void Register(string name, object inst)
    {
        if (!dict.ContainsKey(name))
        {
            dict[name] = inst;
        }
    }

    public static void Register(Type type, object inst)
    {
        string name = type.GetType().ToString();
        if (!dict.ContainsKey(name))
        {
            dict[name] = inst;
        }
    }

    public static void Register<T>(object inst) where T : class
    {
        string name = typeof(T).ToString();
        if (!dict.ContainsKey(name))
        {
            dict[name] = inst;
        }
    }

    public static void Register(object inst)
    {
        if (!dict.ContainsKey(inst.GetType().ToString()))
        {
            dict[inst.GetType().ToString()] = inst;
        }
    }

    public static T Find<T>(string name) where T : class
    {
        return dict[name] as T;
    }

    public static T Find<T>() where T : class
    {
        foreach (KeyValuePair<string, object> kvp in dict)
        {
            if ((kvp.Key == typeof(T).ToString()))
            {
                return kvp.Value as T;
            }
        }
        return default(T);
    }
}

