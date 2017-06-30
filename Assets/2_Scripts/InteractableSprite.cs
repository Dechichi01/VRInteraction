using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InteractableSprite : MonoBehaviour {

    [SerializeField] private Interactable prefab;
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

    public Interactable InstantiatePrefab(Vector3 position, Quaternion rotation)
    {
        return Instantiate(prefab, position, rotation);
    }

    public Interactable InstantiatePrefab()
    {
        return Instantiate(prefab);
    }
}
