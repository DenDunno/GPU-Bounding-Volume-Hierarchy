using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private ComputeShader _computeShader;
        [SerializeField] private int[] _ints;
        private ComputeBuffer _computeBuffer;
        private Kernel _kernel;
        
        private void Start()
        {
            _kernel = new Kernel(_computeShader, "PrefixSum", new Vector3Int(_ints.Length, 1, 1));
            _computeBuffer = new ComputeBuffer(_ints.Length, sizeof(int) * _ints.Length);
            _computeBuffer.SetData(_ints);
            _computeShader.SetBuffer(_kernel.ID, "Input", _computeBuffer);
            
            _kernel.Dispatch();
            _computeBuffer.GetData(_ints);
        }

        private void Update()
        {
        }
    }
}