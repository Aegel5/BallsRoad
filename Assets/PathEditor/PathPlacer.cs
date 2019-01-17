using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class PathPlacer : MonoBehaviour {

    public float spacing = .1f;
    public float resolution = 1;

	
	void Start () {
        Debug.Log("PathPlacer Start");
        var points = FindObjectOfType<PathCreator3d>().path.CalculateEvenlySpacedPoints(spacing, resolution);
        foreach (Vector3 p in points)
        {
            Debug.Log($"Create sphere in position {p}");
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            g.transform.position = p;
            g.transform.localScale = Vector3.one * spacing * .5f;
        }
	}



    private void Update()
    {
        
    }


}
