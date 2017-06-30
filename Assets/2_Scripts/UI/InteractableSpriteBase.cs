using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableSpriteBase : MonoBehaviour {

    private SpriteRenderer _spriteRenderer;
    public SpriteRenderer spriteRender
    {
        get
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
            return _spriteRenderer;
        }
    }

    public abstract Interactable InstantiatePrefab(Vector3 position, Quaternion rotation);
    public abstract Interactable InstantiatePrefab();
    public abstract Interactable InstantiatePrefab(VRInteraction caller);
}
