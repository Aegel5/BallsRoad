using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator3d))]
public class PathEditor3d : Editor {

    PathCreator3d creator;
    Path3d path;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Create new"))
        {
            Undo.RecordObject(creator, "Create new");
            creator.CreatePath();
            path = creator.path;
        }

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    void OnSceneGUI()
    {
        //Debug.Log("PathEditor3d OnSceneGUI");
        Input();
        Draw();
    }

    //void Input()
    //{
    //    Event guiEvent = Event.current;
    //    Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

    //    //if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
    //    //{
    //    //    Undo.RecordObject(creator, "Add segment");
    //    //    path.AddSegment(mousePos);
    //    //}
    //}

    void Draw()
    {
        //Debug.Log("PathEditor3d Draw");
        foreach (Segment seg in path.EnumSegments())
        {
            Handles.color = Color.yellow;
            Handles.DrawLine(seg.p1, seg.start);
            Handles.DrawLine(seg.p2, seg.end);
            Handles.DrawBezier(seg.start, seg.end, seg.p1, seg.p2, Color.green, null, 2);
        }

        for (int i = 0; i < path.SegCount; i++)
        {
            Segment seg = path.GetSegment(i);
            Vector3 PathPoint(Vector3 p)
            {
                return Handles.FreeMoveHandle(p, Quaternion.identity, .1f, Vector3.zero, Handles.CylinderHandleCap);
            }

            Handles.color = Color.red;
            if (i == 0)
            {
                seg.start = PathPoint(seg.start);
            }
            seg.end = PathPoint(seg.end);

            Handles.color = Color.blue;

            seg.p1 = PathPoint(seg.p1);
            seg.p2 = PathPoint(seg.p2);

            Undo.RecordObject(creator, "Move point");
            path.ChangeSegment(i, seg);

            //if (path[i] != newPos)
            //{
            //    Undo.RecordObject(creator, "Move point");
            //    path.MovePoint(i, newPos);
            //}
        }
    }

    void Input()
    {
        //Debug.Log("PathEditor3d Input");
        Event guiEvent = Event.current;
        Vector3 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            Undo.RecordObject(creator, "Add segment");
            Debug.Log("PathEditor3d AddSegment");
            path.AddSegment(mousePos);
        }
    }

    void OnEnable()
    {
        creator = (PathCreator3d)target;
        if (creator.path == null)
        {
            creator.CreatePath();
        }
        path = creator.path;
    }
}
