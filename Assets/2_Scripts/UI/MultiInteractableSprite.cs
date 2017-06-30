using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiInteractableSprite : InteractableSpriteBase {

    [SerializeField] private MultInteractableManager prefab;
    private Interactable interactablePrefab;

    public override Interactable InstantiatePrefab(Vector3 position, Quaternion rotation)
    {
        throw new NotImplementedException();
    }

    public override Interactable InstantiatePrefab()
    {
        throw new NotImplementedException();
    }

    public override Interactable InstantiatePrefab(VRInteraction caller)
    {
        Interactable interactable = prefab.GetAppropriateInteractable(caller);
        Interactable[] all = Instantiate(prefab).GetManagedInteractables();

        return Array.Find(all, i => i.GetType() == interactable.GetType());
    }

}
