using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyEvent 
{
    static Dictionary<string, Dictionary<string, Action<object>>> eventDict = new Dictionary<string, Dictionary<string, Action<object>>>();

    public static void clearAllEvent()
    {
        eventDict.Clear();
    }

    public static void addRegister(string eventName, Action<object> func, string key = "default")
    {
        if(eventDict.ContainsKey(eventName))
        {
            eventDict[eventName][key] = func;
        }
        else
        {
            eventDict[eventName] = new Dictionary<string, Action<object>>();

            eventDict[eventName].Add(key, func);
        }
    }

    public static void unRegister(string eventName, string key = "default")
    {
        if (eventDict.ContainsKey(eventName))
        {
            if(eventDict[eventName].ContainsKey(key))
            {
                eventDict[eventName].Remove(key);
                if(eventDict[eventName].Count  == 0)
                {
                    eventDict[eventName].Clear();
                    eventDict[eventName] = null;
                }
            }
        }
    }

    public static void patchEvent(string eventName, object obj = null)
    {
        if(eventDict.ContainsKey(eventName))
        {
            foreach(var item in eventDict[eventName])
            {
                item.Value(obj);
            }
        }
    }
}
