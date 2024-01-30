using Code;
using Code.Utils;
using UnityEngine;

readonly struct FrustumPlane
{
    private readonly Vector3 _point;
    private readonly Vector3 _normal;

    public FrustumPlane(Vector3 normal, Vector3 point)
    {
        _point = point;
        _normal = normal;
    }
    
    public bool OnPositiveOfNormal(Vector3 spherePosition, float radius, string name)
    {
        Vector3 difference = _point - spherePosition;
        float distance = Vector3.Dot(difference, _normal);
        
        Debug.Log(name + " " + (distance < radius));
        
        return distance < radius;
    }
}

struct Frustum
{
    public FrustumPlane Top;
    public FrustumPlane Bottom;

    public FrustumPlane Right;
    public FrustumPlane Left;

    public FrustumPlane Far;
    public FrustumPlane Near;

    public bool IsOutside(Vector3 position, float radius)
    {
        return Top.OnPositiveOfNormal(position, radius, "Top") &&
               Bottom.OnPositiveOfNormal(position, radius, "Bottom") &&
               Right.OnPositiveOfNormal(position, radius, "Right") &&
               Left.OnPositiveOfNormal(position, radius, "Left") &&
               Far.OnPositiveOfNormal(position, radius, "Far") &&
               Near.OnPositiveOfNormal(position, radius, "Near");
    }
};

public class ClusterDebug : MonoBehaviour
{
    [SerializeField] private GameObject _sphere;
    [SerializeField] private int _tileSizeX = 32;
    [SerializeField] private int _tileSizeY = 32;
    [SerializeField] private float _radius = 0.1f;

    private void OnDrawGizmos()
    {
        if (_sphere == null)
            return;
        
        Vector3 farPlaneParams = Camera.main.GetFarClipPlaneParams();
        Vector3 nearPlaneParams = Camera.main.GetNearClipPlaneParams();
        ClusterPointsCalculator farPlaneClusterCalculator = new(farPlaneParams);
        ClusterPointsCalculator nearPlanePointsCalculator = new(nearPlaneParams);

        DrawSubFrustums(nearPlanePointsCalculator, farPlaneClusterCalculator);
    }

    private void DrawSubFrustums(ClusterPointsCalculator nearPlanePointsCalculator, ClusterPointsCalculator farPlaneClusterCalculator)
    {
        for (int i = 0; i < _tileSizeY; ++i)
        {
            float bottomLerp = (float)i / _tileSizeY;
            float topLerp = (float)(i + 1) / _tileSizeY;

            for (int j = 0; j < _tileSizeX; ++j)
            {
                float leftLerp = (float)j / _tileSizeX;
                float rightLerp = (float)(j + 1) / _tileSizeX;

                Gizmos.color = ColorExtensions.GetRandom((i + 1) * (j + 1));
                RectPoints nearPlanePoints = nearPlanePointsCalculator.Evaluate(bottomLerp, topLerp, leftLerp, rightLerp);
                RectPoints farPlanePoints = farPlaneClusterCalculator.Evaluate(bottomLerp, topLerp, leftLerp, rightLerp);
                Frustum subFrustum = EvaluateSubFrustum(nearPlanePoints, farPlanePoints);
                bool isOutside = subFrustum.IsOutside(_sphere.transform.position, 0.5f);

                if (isOutside)
                    Gizmos.color = Gizmos.color.InverseWithoutAlpha();
                
                DrawPlane(nearPlanePoints);
                DrawPlane(farPlanePoints);
                ConnectPlanes(nearPlanePoints, farPlanePoints);
            }
        }
    }

    private void DrawPlane(RectPoints points)
    {
        for (int i = 0, j = 1; i < 4; ++i, ++j)
        {
            if (j >= 4)
                j = 0;
            
            Gizmos.DrawSphere(points[i], _radius);
            Gizmos.DrawLine(points[i], points[j]);
        }
    }

    private void ConnectPlanes(RectPoints nearPlanePoints, RectPoints farPlanePoints)
    {
        for (int i = 0; i < 4; ++i)
        {
            Gizmos.DrawLine(nearPlanePoints[i], farPlanePoints[i]);
        }
    }

    private Frustum EvaluateSubFrustum(RectPoints nearPlanePoints, RectPoints farPlanePoints)
    {
        return new Frustum()
        {
            Top = new FrustumPlane(Vector3.Cross(
                farPlanePoints.TopLeft - nearPlanePoints.TopLeft, 
                nearPlanePoints.TopRight - nearPlanePoints.TopLeft), nearPlanePoints.TopLeft),
            
            Bottom = new FrustumPlane(Vector3.Cross(
                farPlanePoints.BottomLeft - nearPlanePoints.BottomLeft, 
                nearPlanePoints.BottomLeft - nearPlanePoints.BottomRight), nearPlanePoints.BottomLeft),
            
            Left = new FrustumPlane(Vector3.Cross(
                farPlanePoints.BottomLeft - nearPlanePoints.BottomLeft, 
                nearPlanePoints.TopLeft - nearPlanePoints.BottomLeft), nearPlanePoints.BottomLeft),
            
            Right = new FrustumPlane(Vector3.Cross(
                farPlanePoints.BottomRight - nearPlanePoints.BottomRight, 
                nearPlanePoints.BottomRight - nearPlanePoints.TopRight), nearPlanePoints.BottomRight),
            
            Near = new FrustumPlane(new Vector3(0, 0, -1), nearPlanePoints.BottomRight),
            
            Far = new FrustumPlane(new Vector3(0, 0, 1), farPlanePoints.BottomRight),
        };
    }
}