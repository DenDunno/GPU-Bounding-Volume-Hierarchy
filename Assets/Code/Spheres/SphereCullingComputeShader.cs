using Code.Utils.SubFrustums;
using UnityEngine;

namespace Code.RenderFeature
{
    public class SphereCullingComputeShader
    {
        private readonly IntersectingSpheresData _data;
        private readonly ComputeShader _shader;

        public SphereCullingComputeShader(ComputeShader shader, IntersectingSpheresData data)
        {
            _shader = shader;
            _data = data;
        }

        public void Setup()
        {
            _shader.SetBuffer(0, "_ActiveTiles", _data.ActiveTiles);
            _shader.SetBuffer(0, "_Spheres", _data.Spheres);
            _shader.SetBuffer(0, "_SubFrustums", _data.SubFrustums);
            _shader.SetInt("_SpheresCount", _data.SpheresCount);
        }

        public void Dispatch(Transform cameraTransform)
        {
            ClearActiveTiles();
            _shader.SetMatrix("_CameraWorldToLocal", cameraTransform.worldToLocalMatrix);
            
            //_shader.Dispatch(0, _data.TilesCount, _data.SpheresCount, 1);
            HandleViaCPU(cameraTransform);
        }

        private void ClearActiveTiles()
        {
            int[] activeTiles = new int[_data.TilesCount];
            _data.ActiveTiles.SetData(activeTiles);
        }

        private void HandleViaCPU(Transform cameraTransform)
        {
            SphereData[] spheresData = new SphereData[_data.SpheresCount];
            Frustum[] subFrustums = new Frustum[_data.TilesCount];
            int[] activeTiles = new int[_data.TilesCount];

            _data.Spheres.GetData(spheresData);
            _data.SubFrustums.GetData(subFrustums);

            for (int i = 0; i < subFrustums.Length; ++i)
            {
                for (int j = 0; j < _data.SpheresCount; ++j)
                {
                    Vector4 spherePosition = spheresData[j].Position;
                    spherePosition.w = 1;
                    Vector3 sphereCameraSpacePosition = cameraTransform.worldToLocalMatrix * spherePosition;

                    if (subFrustums[i].IsOutside(sphereCameraSpacePosition, spheresData[j].Radius, false) == false)
                    {
                        activeTiles[i]++;
                    }
                }
            }

            _data.ActiveTiles.SetData(activeTiles);
        }
    }
}