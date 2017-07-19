using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OutlineOnInteraction : ShaderOutlineCtrl {

    [SerializeField] private Transform _interactableParent;
    [SerializeField] private bool outlineOnSelect = true;

    private Interactable[] interactables;

    protected override void Awake()
    {
        base.Awake();
        interactables = _interactableParent.GetComponentsInChildren<Interactable>(true);    
    }

    private void OnEnable()
    {
        if (outlineOnSelect)
        {
            foreach (var interactable in interactables)
            {
                interactable.OnSelectAddListener(EnableOutline);
                interactable.OnDeselectAddListener(DisableOutline);
                interactable.OnManipulateAddListener(DisableOutline);
            }
        }
    }

    private void OnDisable()
    {
        foreach (var interactable in interactables)
        {
            interactable.OnSelectRemoveListener(EnableOutline);
            interactable.OnDeselectRemoveListener(DisableOutline);
            interactable.OnManipulateRemoveListener(DisableOutline);
        }
    }

    private void EnableOutline(VRInteraction caller)
    {
        EnableOutline();
    }

    private void DisableOutline(VRInteraction caller)
    {
        DisableOutline();
    }
}
