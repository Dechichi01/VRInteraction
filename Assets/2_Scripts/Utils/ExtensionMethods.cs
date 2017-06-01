using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviorExtensions
{
    public static T GetActiveComponent<T>(this MonoBehaviour mono) where T : MonoBehaviour
    {
        T[] components = mono.GetComponents<T>();
        return System.Array.Find(components, c => c.enabled);
    }

    public static T GetActiveComponent<T>(this GameObject mono) where T : MonoBehaviour
    {
        T[] components = mono.GetComponents<T>();
        return System.Array.Find(components, c => c.enabled);
    }

    public static T GetActiveComponent<T>(this Transform mono) where T : MonoBehaviour
    {
        T[] components = mono.GetComponents<T>();
        return System.Array.Find(components, c => c.enabled);
    }

    public static T GetActiveComponentInChildren<T>(this MonoBehaviour mono) where T : MonoBehaviour
    {
        T[] components = mono.GetComponentsInChildren<T>();
        return System.Array.Find(components, c => c.enabled);
    }

    public static T GetActiveComponentInChildren<T>(this Transform mono) where T : MonoBehaviour
    {
        T[] components = mono.GetComponentsInChildren<T>();
        return System.Array.Find(components, c => c.enabled);
    }

    public static T GetActiveComponentInChildren<T>(this GameObject mono) where T : MonoBehaviour
    {
        T[] components = mono.GetComponentsInChildren<T>();
        return System.Array.Find(components, c => c.enabled);
    }
}
