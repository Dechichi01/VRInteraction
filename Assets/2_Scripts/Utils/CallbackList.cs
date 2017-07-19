using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackList<T> : List<T>
{
    private Action<T> OnAddItem;
    private Action<T> OnRemoveItem;

    #region Callbacks
    public void OnAddItemAddListener(Action<T> action)
    {
        OnAddItem += action;
    }

    public void OnAddItemRemoveListener(Action<T> action)
    {
        OnAddItem -= action;
    }

    public void OnRemoveItemAddListener(Action<T> action)
    {
        OnRemoveItem += action;
    }

    public void OnRemoveItemRemoveListener(Action<T> action)
    {
        OnRemoveItem -= action;
    }
    #endregion

    public new void Add(T item)
    {
        base.Add(item);
        if (OnAddItem != null)
        {
            OnAddItem(item);
        }
    }

    public new void Remove(T item)
    {
        base.Remove(item);
        if (OnRemoveItem != null)
        {
            OnRemoveItem(item);
        }
    }

}
