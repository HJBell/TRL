using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ViewCastInfo
{
    public bool Hit;
    public Vector3 Point;
    public float Dist;
    public float Angle;

    public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
    {
        Hit = _hit;
        Point = _point;
        Dist = _dst;
        Angle = _angle;
    }
}

public struct EdgeInfo
{
    public Vector3 PointA;
    public Vector3 PointB;

    public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
    {
        PointA = _pointA;
        PointB = _pointB;
    }
}

public class LineOfSight : MonoBehaviour {

    public float Range;

    [SerializeField]
    private LayerMask ObstacleMask;
    [SerializeField]
    private float MeshResolution;
    [SerializeField]
    private int EdgeResolveIterations;
    [SerializeField]
    private float EdgeDistThreshold;
    [SerializeField]
    private float MaskCutawayDist = .1f;
    [SerializeField]
    private MeshFilter ViewMeshFilter;

    private Mesh mViewMesh;


    //-----------------------------Unity Functions-----------------------------

    void Awake()
    {
        mViewMesh = new Mesh();
        mViewMesh.name = "View Mesh";
        ViewMeshFilter.mesh = mViewMesh;
    }

    void LateUpdate()
    {
        //DrawFieldOfView();
    }


    //----------------------------Public Functions-----------------------------

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    
    public void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(360f * MeshResolution);
        float stepAngleSize = 360f / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - 360f / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.Dist - newViewCast.Dist) > EdgeDistThreshold;
                if (oldViewCast.Hit != newViewCast.Hit || (oldViewCast.Hit && newViewCast.Hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.PointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.PointA);
                    }
                    if (edge.PointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.PointB);
                    }
                }

            }


            viewPoints.Add(newViewCast.Point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]) + Vector3.forward * MaskCutawayDist;

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        mViewMesh.Clear();

        mViewMesh.vertices = vertices;
        mViewMesh.triangles = triangles;
        mViewMesh.RecalculateNormals();
    }

    //----------------------------Private Functions----------------------------


    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.Angle;
        float maxAngle = maxViewCast.Angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < EdgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.Dist - newViewCast.Dist) > EdgeDistThreshold;
            if (newViewCast.Hit == minViewCast.Hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.Point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.Point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, Range, ObstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * Range, Range, globalAngle);
        }
    }    
}
