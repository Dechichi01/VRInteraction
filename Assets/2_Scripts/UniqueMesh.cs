using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class UniqueMesh : MonoBehaviour {

    private int ownerId;

    private MeshFilter _mf;
    protected MeshFilter mf
    {
        get
        {
            if (_mf == null)
            {
                _mf = GetComponent<MeshFilter>();
            }
            if (_mf == null)
            {
                _mf = gameObject.AddComponent<MeshFilter>();
            }

            return _mf;
        }
    }

    private Mesh _mesh;
    protected Mesh mesh
    {
        get
        {
            bool isOwner = ownerId == gameObject.GetInstanceID();
            if (mf.sharedMesh == null || !isOwner)
            {
                mf.sharedMesh = _mesh = new Mesh();
                ownerId = gameObject.GetInstanceID();
                _mesh.name = "Mesh [" + ownerId + "]";
            }
            return _mesh;
        }
    }

    private MeshRenderer _meshRenderer;
    protected MeshRenderer meshRenderer
    {
        get
        {
            if (_meshRenderer == null)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
            }
            if (_meshRenderer == null)
            {
                _meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }

            return _meshRenderer;
        }
    }
}
