using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InteractableSprite : InteractableSpriteBase {

    [SerializeField] private Interactable prefab;

    public override Interactable InstantiatePrefab(Vector3 position, Quaternion rotation)
    {
        return Instantiate(prefab, position, rotation);
    }

    public override Interactable InstantiatePrefab()
    {
        return InstantiatePrefab(prefab.transform.position, prefab.transform.rotation);
    }

    public override Interactable InstantiatePrefab(VRInteraction caller)
    {
        return InstantiatePrefab();
    }
}
