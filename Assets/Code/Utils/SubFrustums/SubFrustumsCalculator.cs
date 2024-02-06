using UnityEngine;

namespace Code.Utils.SubFrustums
{
    public class SubFrustumsCalculator
    {
        private readonly Vector2Int _tilesSize;
        private readonly Camera _camera;

        public SubFrustumsCalculator(Camera camera, Vector2Int tilesSize)
        {
            _tilesSize = tilesSize;
            _camera = camera;
        }

        public Frustum[] Evaluate()
        {
            Frustum[] subFrustums = new Frustum[_tilesSize.x * _tilesSize.y];
            
            PopulateSubFrustums(subFrustums);

            return subFrustums;
        }

        private void PopulateSubFrustums(Frustum[] subFrustums)
        {
            Vector3 farPlaneParams = _camera.GetFarClipPlaneParams();
            Vector3 nearPlaneParams = _camera.GetNearClipPlaneParams();
            ClipPlanePointsCalculator farPlaneClipPlaneCalculator = new(farPlaneParams);
            ClipPlanePointsCalculator nearPlanePointsCalculator = new(nearPlaneParams);

            for (int i = 0; i < _tilesSize.y; ++i)
            {
                float bottomLerp = (float)i / _tilesSize.y;
                float topLerp = (float)(i + 1) / _tilesSize.y;

                for (int j = 0; j < _tilesSize.x; ++j)
                {
                    float leftLerp = (float)j / _tilesSize.x;
                    float rightLerp = (float)(j + 1) / _tilesSize.x;

                    RectPoints nearPlanePoints = nearPlanePointsCalculator.Evaluate(bottomLerp, topLerp, leftLerp, rightLerp);
                    RectPoints farPlanePoints = farPlaneClipPlaneCalculator.Evaluate(bottomLerp, topLerp, leftLerp, rightLerp);
                    subFrustums[i * _tilesSize.x + j] = EvaluateSubFrustum(nearPlanePoints, farPlanePoints);
                }
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
}