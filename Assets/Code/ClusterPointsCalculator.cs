using Code.Utils;
using UnityEngine;

namespace Code
{
    public readonly struct ClusterPointsCalculator
    {
        private readonly Vector3 _clipPlaneParams;

        public ClusterPointsCalculator(Vector3 clipPlaneParams)
        {
            _clipPlaneParams = clipPlaneParams;
        }

        public RectPoints Evaluate(float bottomLerp, float topLerp, float leftLerp, float rightLerp)
        {
            return new RectPoints(
                EvaluatePoint(leftLerp, topLerp), 
                EvaluatePoint(rightLerp, topLerp),
                EvaluatePoint(rightLerp, bottomLerp), 
                EvaluatePoint(leftLerp, bottomLerp));
        }

        private Vector3 EvaluatePoint(float xLerp, float yLerp)
        {
            Vector3 localPoint = new()
            {
                x = Mathf.Lerp(-_clipPlaneParams.x / 2f, _clipPlaneParams.x / 2f, xLerp), 
                y = Mathf.Lerp(-_clipPlaneParams.y / 2f, _clipPlaneParams.y / 2f, yLerp),
                z = _clipPlaneParams.z
            };

            return localPoint;
        }
    }
}