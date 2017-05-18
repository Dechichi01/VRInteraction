using UnityEngine;
using System.Collections;

public class meshCreator : MonoBehaviour {

	public float width = 50f;
	public float height = 50f;

	void Start()
	{
		MeshFilter mf = GetComponent<MeshFilter> ();
		Mesh mesh = new Mesh ();
		mf.mesh = mesh;

		//Vertices
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3 (0, 0, 0), new Vector3 (width, 0, 0), new Vector3 (0, height, 0), new Vector3 (width, height, 0)
		};

		//Triangles
		int[] tri = new int[6];

		tri [0] = 0;
		tri [1] = 2;
		tri [2] = 1;

		tri [3] = 2;
		tri [4] = 3;
		tri [5] = 1;


		//Normals (only if you want to display the object)

		Vector3[] normals = new Vector3[4];

		normals [0] = -Vector3.forward;
		normals [1] = -Vector3.forward;
		normals [2] = -Vector3.forward;
		normals [3] = -Vector3.forward;


		//UVa (Textures)
		Vector2[] uv = new Vector2[4];

		uv [0] = new Vector2 (0, 0);
		uv [1] = new Vector2 (1, 0);
		uv [2] = new Vector2 (0, 1);
		uv [3] = new Vector2 (1, 1);


		//Assign Arrays!
		mesh.vertices = vertices;
		mesh.triangles = tri;
//		mesh.uv = uv;

	}



}