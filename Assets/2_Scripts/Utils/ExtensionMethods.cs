using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MonoBehaviorExtensions
{
    public static void Invoke(this MonoBehaviour mono, Action action, float delay = 0)
    {
        mono.StartCoroutine(Invoke(action, delay));
    }

    private static IEnumerator Invoke(Action action, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        if (action != null)
        {
            action();
        }
    }

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

public static class GameObjectExtensions
{
    public static void SetLayer(this GameObject go, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer < 0)
        {
            Debug.LogError(String.Format("Layer with name {0} does not exist. {1}", layerName, Constants.ErrorMsgs.LayerMissing));
            return;
        }

        go.layer = layer;
    }
}

public static class CanvasExtensions
{
    public static Vector2 WorldToCanvas(this Canvas canvas, RectTransform canvasRect, Vector3 worldPos, Camera camera = null)
    {
        if (camera == null)
        {
            camera = Camera.main;
        }

        Vector3 viewport_position = camera.WorldToViewportPoint(worldPos);

        return new Vector2((viewport_position.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
                           (viewport_position.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));
    }
}
