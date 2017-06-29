using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class VRWandItemSelector : MonoBehaviour {

    [SerializeField] private Transform rotationContainer;
    [SerializeField] private float radius = .5f;
    [SerializeField] private float minAngle = 80f;
    private CircList<SpriteRenderer> circItems;
    private int choosenIndex;
    private float[] angles;
    private bool rotatingItens = false;

    private void Start()
    {
        circItems = new CircList<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        SetUpItems();
    }

    private void Update()
    {
        if (!rotatingItens && Input.GetKeyDown(KeyCode.RightArrow))
        {
            circItems.MoveForward();
            StartCoroutine(RotateItems());
        }
    }

    private float GetAngleIncrement()
    {
        if (angles != null && angles.Length > 1)
        {
            return angles[1] - angles[0];
        }
        return Mathf.Max(minAngle, 360f / circItems.Count);
    }

    private void SetUpItems()
    {
        float increment = GetAngleIncrement();
        int angleCount = Mathf.CeilToInt(350f / increment);
        angles = new float[angleCount];
        for (int i = 0; i < angleCount; i++)
        {
            angles[i] = i * increment;
        }

        SpriteRenderer[] itemList = circItems.GetArrayForward();

        Vector3 referencePos = transform.position;
        Vector3 referenceDir = transform.forward * radius;
        Vector3 rotationDir = transform.up;

        int count = itemList.Length;
        for (int i = 0; i < count; i++)
        {
            float angle = angles[i % angleCount];
            Vector3 dir = Quaternion.AngleAxis(angle, rotationDir) * referenceDir;
            itemList[i].transform.position = referencePos + dir;
            itemList[i].transform.forward = dir;
            itemList[i].color = itemList[i].color.SetAlfa(0f);
        }

        for (int i = 0; i < angleCount; i++)
        {
            itemList[i].color = itemList[i].color.SetAlfa(1f);
        }
    }

    private SpriteRenderer[] GetVisibleItems(SpriteRenderer[] items)
    {
        return items.Take(angles.Length).ToArray();
    }

    private IEnumerator RotateItems()
    {
        rotatingItens = true;
        SpriteRenderer[] items = circItems.GetArrayForward();
        SpriteRenderer[] visibleItems = GetVisibleItems(items);
        SpriteRenderer[] itemsToShow = Array.FindAll(visibleItems, i => i.color.a < .1f);
        SpriteRenderer[] itemsToHide = Array.FindAll(
            items, i => i.color.a > .9f && 
            !Array.Exists(visibleItems, v => v == i));

        float angleIncrement = GetAngleIncrement();
        float percent = 0f;
        float speed = 1 / .5f;
        Quaternion startRot = rotationContainer.rotation;
        Quaternion endRot = Quaternion.AngleAxis(angleIncrement, rotationContainer.up) * startRot;

        while (percent < 1f)
        {
            percent += speed * Time.unscaledDeltaTime;
            rotationContainer.rotation = Quaternion.Lerp(startRot, endRot, percent);
            float alfa = Mathf.Lerp(0f, 1f, percent);
            //Array.ForEach(itemsToShow, i => i.color = i.color.SetAlfa(alfa));
            //Array.ForEach(itemsToHide, i => i.color = i.color.SetAlfa(1 - alfa));
            yield return null;
        }

        rotationContainer.rotation = endRot;
        //Array.ForEach(itemsToShow, i => i.color = i.color.SetAlfa(1));
        //Array.ForEach(itemsToHide, i => i.color = i.color.SetAlfa(0));
        rotatingItens = false;
    }
}
