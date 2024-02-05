using Code.Utils;
using Code.Utils.SubFrustums;
using UnityEngine;

public class ClusterDebug : MonoBehaviour
{
    [SerializeField] private GameObject _sphere;
    [SerializeField] private int _tileSizeX = 32;
    [SerializeField] private int _tileSizeY = 32;
    [SerializeField] private float _radius = 0.1f;
    [SerializeField] private bool _showActiveSubFrustums = true;
    [SerializeField] private bool _showNearClip = true;
    [SerializeField] private bool _showFarClip = true;
    [SerializeField] private bool _showFrustum = true;
    
    private void OnDrawGizmos()
    {
        if (_sphere == null)
            return;
        
        Vector3 farPlaneParams = Camera.main.GetFarClipPlaneParams();
        Vector3 nearPlaneParams = Camera.main.GetNearClipPlaneParams();
        ClipPlanePointsCalculator farPlaneClipPlaneCalculator = new(farPlaneParams);
        ClipPlanePointsCalculator nearPlanePointsCalculator = new(nearPlaneParams);

        DrawSubFrustums(nearPlanePointsCalculator, farPlaneClipPlaneCalculator);
    }

    private void DrawSubFrustums(ClipPlanePointsCalculator nearPlanePointsCalculator, ClipPlanePointsCalculator farPlaneClipPlaneCalculator)
    {
        SubFrustumsCalculator subFrustumsCalculator = new(Camera.main, _tileSizeX, _tileSizeY);
        Frustum[] subFrustums = subFrustumsCalculator.Evaluate();
        
        for (int i = 0; i < _tileSizeY; ++i)
        {
            float bottomLerp = (float)i / _tileSizeY;
            float topLerp = (float)(i + 1) / _tileSizeY;

            for (int j = 0; j < _tileSizeX; ++j)
            {
                float leftLerp = (float)j / _tileSizeX;
                float rightLerp = (float)(j + 1) / _tileSizeX;
                
                RectPoints nearPlanePoints = nearPlanePointsCalculator.Evaluate(bottomLerp, topLerp, leftLerp, rightLerp);
                RectPoints farPlanePoints = farPlaneClipPlaneCalculator.Evaluate(bottomLerp, topLerp, leftLerp, rightLerp);

                Vector4 point = new Vector4(_sphere.transform.position.x, _sphere.transform.position.y, _sphere.transform.position.z, 1);
                Vector3 sphereCameraSpacePosition = Camera.main.transform.worldToLocalMatrix * point;
                bool isOutside = subFrustums[i * _tileSizeX + j].IsOutside(sphereCameraSpacePosition, 0.5f);
                Gizmos.color = isOutside ? Color.red : Color.green;
                
                if (_showActiveSubFrustums || isOutside == false)
                {
                    if (_showNearClip)
                        DrawPlane(nearPlanePoints);
                    
                    if (_showFarClip)
                        DrawPlane(farPlanePoints);
                    
                    if (_showFrustum)
                        ConnectPlanes(nearPlanePoints, farPlanePoints);
                }
            }
        }
    }

    private void DrawPlane(RectPoints points)
    {
        for (int i = 0, j = 1; i < 4; ++i, ++j)
        {
            if (j >= 4)
                j = 0;
            
            Gizmos.DrawSphere(transform.TransformPoint(points[i]), _radius);
            Gizmos.DrawLine(transform.TransformPoint(points[i]), transform.TransformPoint(points[j]));
        }
    }

    private void ConnectPlanes(RectPoints nearPlanePoints, RectPoints farPlanePoints)
    {
        for (int i = 0; i < 4; ++i)
        {
            Gizmos.DrawLine(transform.TransformPoint(nearPlanePoints[i]), transform.TransformPoint(farPlanePoints[i]));
        }
    }
}