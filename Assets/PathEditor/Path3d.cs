using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Segment
{
    public Vector3 start;
    public Vector3 end;

    public Vector3 p1Delta;
    public Vector3 p2Delta;

    public Vector3 p1
    { get { return start + p1Delta; }
        set { p1Delta = value-start; }
    }
    public Vector3 p2 { get { return end + p2Delta; } set { p2Delta = value - end; } }

    public Segment Clone()
    {
        return (Segment)MemberwiseClone();
    }

}

[System.Serializable]
public class Path3d
{
    [SerializeField, HideInInspector]
    List<Segment> segments = new List<Segment>();

    public Path3d(Vector3 center)
    {
        Segment seg = new Segment
        {
            start = center + Vector3.left,
            end = center + Vector3.right,

            p1Delta = (Vector3.left + Vector3.up) * .5f,
            p2Delta = (Vector3.right + Vector3.down) * .5f,
        };

        segments.Add(seg);
    }

    public int SegCount { get { return segments == null ? 0 : segments.Count; } }

    public Segment GetSegment(int i)
    {
        return segments[i].Clone();
    }

    //List<Segment> GetSegments()
    //{
    //    return segments;
    //}

    public void ChangeSegment(int i, Segment seg)
    {
        Segment old = segments[i];
        if(i > 0)
        {
            segments[i - 1].end = seg.start;
        }
        if(i < segments.Count - 1)
        {
            segments[i + 1].start = seg.end;
        }
        segments[i] = seg;
    }

    public void AddSegment(Vector3 pos)
    {
        Segment seg = new Segment();
        if (segments.Any())
            seg.start = segments.Last().end;
        else
            seg.start = seg.end - Vector3.forward*3;
        seg.end = pos;

        segments.Add(seg);

    }

    public IEnumerable<Segment> EnumSegments()
    {
        if(segments == null)
        {
            Debug.Log("seg == null");
            yield break;
        }
        foreach(var elem in segments)
        {
            yield return elem.Clone();
        }
        yield break;
    }

    public List<Vector3> CalculateEvenlySpacedPoints(float spacing, float resolution = 1)
    {
        List<Vector3> evenlySpacedPoints = new List<Vector3>();
        for (int i =0; i< segments.Count; i++)
        {
            bool isLast = i == segments.Count - 1;
            Segment seg = segments[i];
            int count = 20;
            double step = 1 / count;
            for(int j = 0; j < count; j++)
            {
                float t = (float)(j * step);
                if (j == count - 1)
                    t = 1;
                Debug.Log($"t={t}");
                var point = Bezier.EvaluateCubic(seg.start, seg.p1, seg.p2, seg.end, t);

                if (t >= 1)
                {
                    if (isLast)
                        evenlySpacedPoints.Add(point);
                }
                else
                {
                    evenlySpacedPoints.Add(point);
                    //Debug.Log($"add pint {point}");
                }
            }
        }

        return evenlySpacedPoints;
    }







}
