using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MultInteractableManager : MonoBehaviour {

    [SerializeField] private Interactable[] managedInteractables;

    private void OnEnable()
    {
        foreach (var interactable in managedInteractables)
        {
            interactable.OnManipulationStartAddListener(OnInteractableStartManipulation);
            interactable.OnManipulationEndAddListener(OnInteractableEndManipulation);
        }
    }

    private void OnDisable()
    {
        foreach (var interactable in managedInteractables)
        {
            interactable.OnManipulationStartRemoveListener(OnInteractableStartManipulation);
            interactable.OnManipulationEndRemoveListener(OnInteractableEndManipulation);
        }
    }

    private void OnInteractableStartManipulation(VRInteraction caller)
    {
        foreach (var interactable in managedInteractables)
        {
            if (!interactable.isManipulated)
            {
                interactable.enabled = false;
            }
        }
    }

    private void OnInteractableEndManipulation(VRInteraction caller)
    {
        if (!Array.Exists(managedInteractables, i => i.isManipulated))
        {
            Array.ForEach(managedInteractables, i => i.enabled = true);
        }
    }
}


