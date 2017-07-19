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
        System.Array.ForEach(interactables, i => i.OnManipulateAddListener(EnableSelectorInteraction));
        System.Array.ForEach(interactables, i => i.OnReleaseAddListener(DisableSelectorInteraction));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
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

    private IEnumerator UpdateDisplay()
    {
        char[] unities = new char[3] { 'V', 'A', 'Ω' };
        while (enabled)
        {
            float displayVal = 0f;
            if (!highConnection.isAvailable && !groundConnection.isAvailable)
            {
                displayVal = highConnection.voltage - groundConnection.voltage;
            }

            display.text = string.Format("{0:f2} <size=5>{1}</size>", displayVal, unities[(int)currentMode]);
            yield return new WaitForSeconds(.1f);
        }
    }

    private enum DisplayMode { Voltage = 0, Current = 1, Resistance = 2}
}
