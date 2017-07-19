using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShaderChange : MonoBehaviour {

    [SerializeField] MeshRenderer meshRend;
    [SerializeField] private Shader[] newShaders;
    private Shader[] defaultShaders;
    private Material[] materials;

    private void Awake()
    {
        materials = meshRend.sharedMaterials;
        defaultShaders = materials.Select(m => m.shader).ToArray();
    }

    public void ChangeShader()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].shader = newShaders[i%newShaders.Length];
        }
    }

    public void RestoreShader()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].shader = defaultShaders[i];
        }
    }

}
