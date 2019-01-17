using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator3d : MonoBehaviour {

    [HideInInspector]
    public Path3d path;

    public void CreatePath()
    {
        path = new Path3d(transform.position);
    }
}
