using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MultInteractableManager : MonoBehaviour {

    //MUDAR TUDO, INTERAGIR COM O INTERACTABLE, NÃO COM O MANAGER
    [SerializeField] private GameObject interactionManager;
    [SerializeField] private ManagedInteractable[] managedInteractables;

    private IVRSelectionManager selectManager;

    public IVRSelectionManager GetSelectManager() { return selectManager; }
    public void SetSelectManager(IVRSelectionManager selectManager)
    {
        if (Application.isEditor)
        {
            this.selectManager = selectManager;
        }
    }

    private void Awake()
    {
        selectManager = interactionManager.GetComponent<IVRSelectionManager>();
    }

    private void OnEnable()
    {
        selectManager.OnChangeInteractionAddListener(OnChangeInteraction);
    }

    private void OnDisable()
    {
        selectManager.OnChangeInteractionRemoveListener(OnChangeInteraction);
    }

    private void OnChangeInteraction(InteractionType intType)
    {
        Interactable interactableToEnable = Array.Find(managedInteractables, m => m.interactionType == intType).interactable;
        if (interactableToEnable != null)
        {
            Array.ForEach(managedInteractables, m => m.interactable.enabled = false);
            interactableToEnable.enabled = true;
        }
    }

    [Serializable]
	private struct ManagedInteractable
    {
        public Interactable interactable;
        public InteractionType interactionType;
    }
}


