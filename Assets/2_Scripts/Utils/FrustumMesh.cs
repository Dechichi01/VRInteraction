using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustumMesh : UniqueMesh {

    [SerializeField] [HideInInspector] private float _distance = 10f;
    public float distance
    {
        get { return _distance; }
        set
        {
            _distance = value;
        }
    }

    [SerializeField] [HideInInspector] private float _halfAngle = 20f;
    public float halfAngle
    {
        get { return _halfAngle; }
        set
        {
            _halfAngle = Mathf.Clamp(value, 0f, 90f);
        }
    }

    public Mesh GetMesh()
    {
        return mesh;
    }

    public MeshRenderer GetMeshRenderer()
    {
        return meshRenderer;
    }

    public void GenerateMesh()
    {
        float halfHeight = Mathf.Tan(Mathf.Deg2Rad * halfAngle) * distance;
        Vector3 startPoint = Vector3.zero;
        Vector3 endPoint = startPoint + Vector3.forward * distance;

        Vector3[] vertices = new Vector3[5]
        {
            startPoint,
            endPoint - Vector3.right*halfHeight + Vector3.up*halfHeight,
            endPoint + Vector3.right*halfHeight + Vector3.up*halfHeight,
            endPoint + Vector3.right*halfHeight - Vector3.up*halfHeight,
            endPoint - Vector3.right*halfHeight - Vector3.up*halfHeight,
        };

        int[] triangles = new int[6 * 3]
        {
            0,1,2,
            0,2,3,
            0,4,3,
            0,4,1,
            1,3,2,
            1,4,3
        };

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        meshRenderer.sharedMaterial = new Material(Shader.Find("Transparent/Diffuse"));
        meshRenderer.sharedMaterial.color = new Color(0f, 1f, 0f, .5f);
    }
}
