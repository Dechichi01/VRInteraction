using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class CircList<T> : IEnumerable<T>
{
    private List<T> elements = new List<T>();

    public T current { get; private set; }
    public T next { get; private set; }
    public T previous { get; private set; }

    public int Count { get { return elements.Count; } }
    public CircList(IEnumerable<T> newElements)
    {
        elements = new List<T>(newElements);
        if (elements.Count == 0) return;

        current = elements[0];
        next = elements[1 % elements.Count];
        previous = elements[elements.Count - 1];
    }

    public void MoveForward()
    {
        T temp = current;
        current = next;
        previous = temp;

        next = elements[(elements.IndexOf(next) + 1) % elements.Count];
    }

    public void MoveBackwards()
    {
        T temp = current;
        current = previous;
        next = temp;

        int prevIndex = elements.IndexOf(previous) -1;
        if (prevIndex < 0) prevIndex = elements.Count - 1;

        previous = elements[prevIndex];
    }

    public T[] GetArrayForward()
    {
        return GetArray();
    }

    public T[] GetArrayBackwards()
    {
        elements.Reverse();
        T[] result = GetArray();
        elements.Reverse();

        return result;
    }

    private T[] GetArray()
    {
        T[] result = new T[elements.Count];
        int currIndex = elements.IndexOf(current);
        int count = result.Length;
        for (int i = 0; i < count; i++)
        {
            result[i] = elements[currIndex];
            currIndex = (currIndex + 1) % count;
        }

        return result;
    }

    public T[] PickFromCurrent(int previousNumber, int nextNumber)
    {
        if (previousNumber + nextNumber + 1 > elements.Count)
        {
            previousNumber = nextNumber = (elements.Count - 1) / 2;
        }

        int startIndex = elements.IndexOf(current) - previousNumber;
        if (startIndex < 0)
        {
            startIndex = elements.Count +startIndex;
        }

        int count = previousNumber + nextNumber + 1;
        int elementsCount = elements.Count;
        T[] result = new T[count];

        for (int i = 0; i < count; i++)
        {
            result[i] = elements[(startIndex + i) % elementsCount];
        }

        return result;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return elements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return elements.GetEnumerator();
    }

}
