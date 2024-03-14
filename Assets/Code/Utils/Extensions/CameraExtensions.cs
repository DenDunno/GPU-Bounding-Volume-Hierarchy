using UnityEngine;

namespace Code.Utils.Extensions
{
    public static class CameraExtensions
    {
        public static Vector3 GetNearClipPlaneParams(this Camera camera)
        {
            return GetClipPlaneSize(camera, camera.nearClipPlane);
        }
        
        public static Vector3 GetFarClipPlaneParams(this Camera camera)
        {
            return GetClipPlaneSize(camera, camera.farClipPlane);
        }

        private static Vector3 GetClipPlaneSize(this Camera camera, float depth)
        {
            Vector3 bottomLeftPoint = camera.ViewportToWorldPoint(new Vector3(0, 0, depth));
            Vector3 bottomRightPoint = camera.ViewportToWorldPoint(new Vector3(1, 0, depth));
            Vector3 upperLeftPoint = camera.ViewportToWorldPoint(new Vector3(0, 1, depth));

            float planeHeight = (bottomLeftPoint - upperLeftPoint).magnitude;
            float planeWidth = (bottomLeftPoint - bottomRightPoint).magnitude;

            return new Vector3(planeWidth, planeHeight, depth);
        }
    }
}