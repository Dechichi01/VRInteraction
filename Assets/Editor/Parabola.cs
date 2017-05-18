using UnityEditor;
using UnityEngine;

public class Parabola {
	#region Private Attributes
	//TODO: usuário informar no. de segmentos?
	private int sections = 20;
	private LineRenderer lineRenderer;
	#endregion
	
	
	#region Public Methods
	public Parabola (LineRenderer lineRenderer) {
		this.lineRenderer = lineRenderer;
		this.lineRenderer.SetVertexCount(sections);
	}
	
	public void Plot(Vector3 p0, Vector3 c0, Vector3 p1) {
   		float t;
		Vector3 coords;
   		for(int i = 0; i < sections; i++ ) {
      		t = (float) i / (sections - 1);
			coords = getQuadraticCoordinates(t, p0, c0, p1);
      		lineRenderer.SetPosition (i, coords);
   		}
	}
	#endregion
	
	
	#region Private Methods
	private Vector3 getQuadraticCoordinates(float t, Vector3 p0, Vector3 c0, Vector3 p1) {
		return Mathf.Pow(1-t,2) * p0 + 2*t*(1-t) * c0 + Mathf.Pow(t,2) * p1;
	}
	#endregion
}

