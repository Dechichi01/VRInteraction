using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderOutlineCtrl : MonoBehaviour {

    [SerializeField] private MeshRenderer meshRend;
    [SerializeField] private Material outlineMaterial;

    private Material[] defaultMaterials;
    private Material[] materialsWithOutline;

    protected virtual void Awake()
    {
        defaultMaterials = meshRend.materials;
        materialsWithOutline = new Material[defaultMaterials.Length + 1];
        System.Array.Copy(defaultMaterials, materialsWithOutline, defaultMaterials.Length);
        materialsWithOutline[materialsWithOutline.Length - 1] = outlineMaterial;
    }

    public void EnableOutline()
    {
        meshRend.materials = materialsWithOutline;
    }

    public void DisableOutline()
    {
        meshRend.materials = defaultMaterials;
    }
}
