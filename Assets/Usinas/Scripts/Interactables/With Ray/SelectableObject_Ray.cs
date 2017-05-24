/*using UnityEngine;
using System.Collections;
using System;

public abstract class SelectableObject_Ray : Interactable {

    public Shader selectedShader;
    protected Shader[] baseShader;

    public MeshRenderer meshRenderer;

    override protected void Start()
    {
        gameObject.tag = "SelectableObject";
        canInteract = true;

        if (meshRenderer != null)
        {
            Material[] materials = meshRenderer.materials;
            baseShader = new Shader[materials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                baseShader[i] = materials[i].shader;
            }
        }
    }

    public void ChangeToSelectedShader()
    {
        if (meshRenderer == null || selectedShader == null) return;
        if (canInteract)
        {
            Material[] materials = meshRenderer.materials;
            Renderer renderer = meshRenderer.transform.GetComponent<Renderer>();
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].shader = selectedShader;
                renderer.materials[i].SetColor("_OutlineColor", new Color(0, 255, 0, 1));
            }

        }

    }

    public void ChangeToBaseShader()
    {
        if (meshRenderer == null) return;
        if (canInteract)
        {
            Material[] materials = meshRenderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].shader = baseShader[i];
            }
        }
    }

    public override void OnSelected()
    {
        ChangeToSelectedShader();
    }

    public override void OnDeselect()
    {
        ChangeToBaseShader();
    }
}
*/