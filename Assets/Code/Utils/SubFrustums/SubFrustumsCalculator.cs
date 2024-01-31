using UnityEngine;

namespace Code.Utils.SubFrustums
{
    public class SubFrustumsCalculator
    {
        private readonly Camera _camera;
        private readonly int _countX;
        private readonly int _countY;

        public SubFrustumsCalculator(Camera camera, int countX, int countY)
        {
            _camera = camera;
            _countX = countX;
            _countY = countY;
        }

        public Frustum[] Evaluate()
        {
            Frustum[] subFrustums = new Frustum[_countX * _countY];
            
            PopulateSubFrustums(subFrustums);

            return subFrustums;
        }

        private void PopulateSubFrustums(Frustum[] subFrustums)
        {
            Vector3 farPlaneParams = _camera.GetFarClipPlaneParams();
            Vector3 nearPlaneParams = _camera.GetNearClipPlaneParams();
            ClipPlanePointsCalculator farPlaneClipPlaneCalculator = new(farPlaneParams);
            ClipPlanePointsCalculator nearPlanePointsCalculator = new(nearPlaneParams);

            for (int i = 0; i < _countY; ++i)
            {
                float bottomLerp = (float)i / _countY;
                float topLerp = (float)(i + 1) / _countY;

                for (int j = 0; j < _countX; ++j)
                {
                    float leftLerp = (float)j / _countX;
                    float rightLerp = (float)(j + 1) / _countX;

                    RectPoints nearPlanePoints = nearPlanePointsCalculator.Evaluate(bottomLerp, topLerp, leftLerp, rightLerp);
                    RectPoints farPlanePoints = farPlaneClipPlaneCalculator.Evaluate(bottomLerp, topLerp, leftLerp, rightLerp);
                    subFrustums[i * _countX + j] = EvaluateSubFrustum(nearPlanePoints, farPlanePoints);
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