using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Multimeter : MonoBehaviour {

    [SerializeField] private MultimeterSelector selector;
    [SerializeField] private Text display;
    [SerializeField] private EletricConnection highConnection;
    [SerializeField] private EletricConnection groundConnection;

    [SerializeField] private Range voltageRange;
    [SerializeField] private Range currentRange;
    [SerializeField] private Range resistanteRange;

    private Interactable[] interactables;

    private DisplayMode currentMode;

    private void Awake()
    {
        selector.enabled = false;
        interactables = GetComponentsInChildren<Interactable>().Where(i => i != selector).ToArray();
    }

    private void OnEnable()
    {
        StartCoroutine(UpdateDisplay());

        selector.OnManipulateAddListener(OnSelectorHeld);
        System.Array.ForEach(interactables, i => i.OnManipulateAddListener(EnableSelectorInteraction));
        System.Array.ForEach(interactables, i => i.OnReleaseAddListener(DisableSelectorInteraction));
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        selector.OnManipulateRemoveListener(OnSelectorHeld);
        System.Array.ForEach(interactables, i => i.OnManipulateRemoveListener(EnableSelectorInteraction));
        System.Array.ForEach(interactables, i => i.OnReleaseRemoveListener(DisableSelectorInteraction));
    }

    private void EnableSelectorInteraction(VRInteraction caller)
    {
        selector.enabled = true;
    }

    private void DisableSelectorInteraction(VRInteraction caller)
    {
        if (selector.isBeingHeld)
        {
            selector.ReleaseHand();
        }
        selector.enabled = false;
    }

    private void OnSelectorHeld(VRInteraction caller)
    {
        StartCoroutine(UpdateMode());
    }

    private float GetDisplayVal()
    {
        if (!highConnection.isAvailable && !groundConnection.isAvailable)
        {
            switch (currentMode)
            {
                case DisplayMode.Voltage:
                    return highConnection.voltage - groundConnection.voltage;
                case DisplayMode.Current:
                    return highConnection.current;
                case DisplayMode.Resistance:
                    return (highConnection.voltage - groundConnection.voltage) / highConnection.current;
            }
        }

        return 0f;
    }

    private IEnumerator UpdateDisplay()
    {
        char[] unities = new char[3] { 'V', 'A', 'Ω' };
        while (enabled)
        {
            display.text = string.Format("{0:f2} <size=5>{1}</size>", GetDisplayVal(), unities[(int)currentMode]);
            yield return new WaitForSeconds(.1f);
        }
    }

    private IEnumerator UpdateMode()
    {
        Range[] ranges = new Range[3] { voltageRange, currentRange, resistanteRange };
        float zRot = 0;
        int index = 0;

        yield return new WaitForEndOfFrame();

        while (selector.isBeingHeld)
        {
            zRot = MathUtils.AbsAngle(selector.zRotation);
            index = Mathf.Max(0, System.Array.FindIndex(ranges, r => zRot >= r.from && zRot < r.to));

            if (index != (int) currentMode)
            {
                currentMode = (DisplayMode)index;
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    private enum DisplayMode { Voltage = 0, Current = 1, Resistance = 2}

    [System.Serializable]
    private struct Range
    {
        public float from;
        public float to;
    }
}
