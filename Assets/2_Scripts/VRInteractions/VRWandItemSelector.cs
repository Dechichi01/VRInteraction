using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(VRInputManager))]
public class VRWandItemSelector : MonoBehaviour {

    [SerializeField] private Transform rotationContainer;
    [SerializeField] private float _radius = .5f;
    [SerializeField] [Range(0,10)] private int itemsShown = 3;
    [SerializeField] [Range(10, 360)] private float angleDelta = 30;
    [SerializeField] private float turnTime = .5f;

    private float radius { get { return _radius * .01f; } }
    private int halfItems { get { return (itemsShown - 1) / 2; } }
    private CircList<SpriteRenderer> circItems;

    private SpriteRenderer[] allItems;
    private SpriteRenderer[] currentVisibles;
    private float[] visibleAngles;
    private float[] angles;

    private VRInputManager inputManager;
    private VRInteraction[] otherInteractions;

    private bool hidden = false;
    private bool rotating = false;
    private bool selectionChanging = false;

    private void Start()
    {
        inputManager = GetComponent<VRInputManager>();
        otherInteractions = GetComponentsInChildren<VRInteraction>();
        allItems = rotationContainer.GetComponentsInChildren<SpriteRenderer>();
        circItems = new CircList<SpriteRenderer>(allItems);
        SetUpItems();
    }

    private void Update()
    {
        if (inputManager.GetPressDown(VRInput.Vive.menuButton))
        {
            if (hidden)
            {
                ShowSelection();
            }
            else
            {
                HideSelection();
            }
        }

        if (!hidden)
        {
            if (inputManager.TrackPadPress(TrackPadXAxis.Right,TrackPadYAxis.None))
            {
                RotateItems(1);
            }
            else if (inputManager.TrackPadPress(TrackPadXAxis.Left, TrackPadYAxis.None))
            {
                RotateItems(-1);
            }
        }
    }

    private void RotateItems(int direction)
    {
        if (rotating || hidden)
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

        SetGroupCircle(currentVisibles, visibleAngles, radius);
        hidden = true;
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

    private void HideSelection()
    {
        if (hidden || rotating || selectionChanging)
        {
            return;
        }

        foreach (var other in otherInteractions)
        {
            other.enabled = true;
        }

        selectionChanging = true;
        StartCoroutine(HideShowItems(1f, 0f));
    }

    private void ShowSelection()
    {
        if (!hidden || selectionChanging)
        {
            return;
        }

        foreach (var other in otherInteractions)
        {
            other.SetManipulatedInteractable(null);
            other.enabled = false;
        }

        selectionChanging = true;
        StartCoroutine(HideShowItems(0f, 1f));
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

    private IEnumerator HideShowItems(float startPercent, float endPercent)
    {
        Vector3 startScale = Vector3.one * startPercent;
        Vector3 endScale = Vector3.one * endPercent;
        float startRadius = Mathf.Max(radius*.5f, radius * startPercent);
        float endRadius = Mathf.Max(radius*.5f, radius * endPercent);

        float percent = 0f;
        float speed = 1f / turnTime;

        SpriteRenderer[] items = circItems.PickFromCurrent(halfItems, halfItems);
        Vector3 center = rotationContainer.position;
        
        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            foreach (var item in items)
            {
                item.transform.position = center + (item.transform.position - center).normalized * Mathf.Lerp(startRadius, endRadius, percent);  
                item.color = ColorUtils.SetAlfa(item.color, Mathf.Lerp(startPercent, endPercent, percent));
            }

            yield return null;
        }

        hidden = endPercent < startPercent;
        selectionChanging = false;
    }

}
