using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VRWandItemSelector : MonoBehaviour {

    [SerializeField] private Transform rotationContainer;
    [SerializeField] private float radius = .5f;
    [SerializeField] [Range(0,10)] private int itemsShown = 3;
    [SerializeField] [Range(10, 360)] private float angleDelta = 30;
    [SerializeField] private float turnTime = .5f;

    private int halfItems { get { return (itemsShown - 1) / 2; } }
    private CircList<SpriteRenderer> circItems;

    private SpriteRenderer[] allItems;
    private SpriteRenderer[] currentVisibles;
    private float[] visibleAngles;
    private float[] angles;

    private bool rotating = false;

    private void Start()
    {
        allItems = GetComponentsInChildren<SpriteRenderer>();
        circItems = new CircList<SpriteRenderer>(allItems);
        SetUpItems();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RotateItems(1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RotateItems(-1);
        }
    }

    private void RotateItems(int direction)
    {
        if (rotating)
        {
            return;
        }

        rotating = true;

        direction = (int) Mathf.Sign(direction);

        SpriteRenderer newInvisible;
        SpriteRenderer newVisible;
        SpriteRenderer[] all = new SpriteRenderer[currentVisibles.Length + 1];
        float[] previousAngles = new float[angles.Length-1];
        float[] newAngles = new float[angles.Length - 1];

        if (direction > 0)
        {
            newInvisible = all[0] = currentVisibles[0];
            circItems.MoveForward();
            currentVisibles = circItems.PickFromCurrent(halfItems, halfItems);
            newVisible = currentVisibles[currentVisibles.Length - 1];

            Array.Copy(currentVisibles, 0, all, 1, currentVisibles.Length);
            Array.Copy(angles, 1, previousAngles, 0, previousAngles.Length);
            Array.Copy(angles, 0, newAngles, 0, newAngles.Length);
        }
        else
        {
            newInvisible = all[all.Length - 1] = currentVisibles[currentVisibles.Length - 1];
            circItems.MoveBackwards();
            currentVisibles = circItems.PickFromCurrent(halfItems, halfItems);
            newVisible = currentVisibles[0];

            Array.Copy(currentVisibles, 0, all, 0, currentVisibles.Length);

            Array.Copy(angles, 0, previousAngles, 0, previousAngles.Length);
            Array.Copy(angles, 1, newAngles, 0, newAngles.Length);
        }

        StartCoroutine(PerformItemRotation(all, newVisible, newInvisible, previousAngles, newAngles));
    }

    private void SetUpItems()
    {
        SetUpAngles();

        for (int i = 0; i < allItems.Length; i++)
        {
            allItems[i].color = ColorUtils.SetAlfa(allItems[i].color, 0);
        }

        currentVisibles = circItems.PickFromCurrent(halfItems, halfItems);

        for (int i = 0; i < currentVisibles.Length; i++)
        {
            currentVisibles[i].color = ColorUtils.SetAlfa(currentVisibles[i].color, 1);
        }

        SetGroupCircle(currentVisibles, visibleAngles, radius);
    }

    private void SetUpAngles()
    {
        angles = new float[itemsShown + 2];

        int aux = -halfItems -1;
        for (int i = 0; i < angles.Length; i++)
        {
            angles[i] = aux * angleDelta;
            aux++;
        }

        visibleAngles = new float[angles.Length - 2];
        Array.Copy(angles, 1, visibleAngles, 0, visibleAngles.Length);
    }

    private void SetGroupCircle(SpriteRenderer[] items, float[] angles, float radius)
    {
        int count = angles.Length;
        Vector3 center = rotationContainer.position;
        for (int i = 0; i < count; i++)
        {
            float angle = angles[i];
            Transform t = items[i].transform;
            t.position = center + Quaternion.AngleAxis(angle, rotationContainer.up) * rotationContainer.forward * radius;
            t.localRotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private void LerpAngleGroup(float[] result, float[] start, float[] end, float percent)
    {
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = Mathf.LerpAngle(start[i], end[i], percent);
        }
    }

    private IEnumerator PerformItemRotation(SpriteRenderer[] allItems, SpriteRenderer newVisible, SpriteRenderer newInvisible, float[] previousAngles, float[] newAngles)
    {
        float[] lerpAngles = new float[previousAngles.Length];

        float percent = 0;
        float speed = 1f / turnTime;

        Action<float> SetItemsCircle = (p) =>
        {
            LerpAngleGroup(lerpAngles, previousAngles, newAngles, p);
            SetGroupCircle(allItems, lerpAngles, radius);
            newVisible.color = ColorUtils.SetAlfa(newVisible.color, Mathf.Lerp(0f, 1f, p));
            newInvisible.color = ColorUtils.SetAlfa(newInvisible.color, Mathf.Lerp(1f, 0f, p));
        };

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            SetItemsCircle(percent);

            yield return null;
        }

        SetItemsCircle(1);

        rotating = false;
    }

}
