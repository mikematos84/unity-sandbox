using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/**
 * 
 * Quicker/simplier form of messages with no generic implementation
 * 
 */
public abstract class Messenger
{

    protected static Dictionary<string, MessageHandler> dict = new Dictionary<string, MessageHandler>();

    public static void SendNote(string note, object val = null)
    {
        if (dict.ContainsKey(note))
        {
            dict[note].Invoke(val);
        }
    }

    public static void ListenTo(string note, MessageHandler handler)
    {
        if (dict.ContainsKey(note))
        {
            dict[note] += handler;
        }
        else
        {
            dict.Add(note, handler);
        }
    }

    public static void StopListeningTo(string note, MessageHandler handler)
    {
        if (dict.ContainsKey(note))
        {
            dict[note] -= handler;
            if (dict[note] == null)
            {
                dict.Remove(note);
            }
        }
    }

    public static void Reset()
    {
        dict.Clear();
    }
}
public delegate void MessageHandler(object o);



// generic version
public abstract class Messenger<T>
{

    protected static Dictionary<int, Dictionary<string, MessageHandler<T>>> dict = new Dictionary<int, Dictionary<string, MessageHandler<T>>>();

    public static void SendNote(string note, T val, int channel = 0)
    {
        if (!dict.ContainsKey(channel))
        {
            dict.Add(channel, new Dictionary<string, MessageHandler<T>>());
        }



        Dictionary<string, MessageHandler<T>> cdict = dict[channel];
        if (cdict.ContainsKey(note))
        {
            cdict[note].Invoke(val);
        }
    }

    public static void ListenTo(string note, MessageHandler<T> handler, int channel = 0)
    {
        if (!dict.ContainsKey(channel))
        {
            dict.Add(channel, new Dictionary<string, MessageHandler<T>>());
        }


        Dictionary<string, MessageHandler<T>> cdict = dict[channel];
        if (cdict.ContainsKey(note))
        {
            cdict[note] += handler;
        }
        else
        {
            cdict.Add(note, handler);
        }
    }

    public static void StopListeningTo(string note, MessageHandler<T> handler, int channel = 0)
    {
        if (!dict.ContainsKey(channel))
        {
            dict.Add(channel, new Dictionary<string, MessageHandler<T>>());
        }


        Dictionary<string, MessageHandler<T>> cdict = dict[channel];
        if (cdict.ContainsKey(note))
        {
            cdict[note] -= handler;
            if (cdict[note] == null)
            {
                cdict.Remove(note);
            }
        }
    }

    public static void Reset()
    {
        dict.Clear();
    }
}
public delegate void MessageHandler<T>(T o);

