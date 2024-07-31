using System.Collections.Generic;
using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private ComputeShader _computeShader;
        [SerializeField] private int[] _ints;
        [SerializeField] [Range(1, 32)] private int _digits = 1;
        private ComputeBuffer _computeBuffer;
        private Kernel _kernel;
        private List<int> _temp;
        
        private void Start()
        {
            _kernel = new Kernel(_computeShader, "PrefixSum", new Vector3Int(_ints.Length, 1, 1));
            _computeBuffer = new ComputeBuffer(_ints.Length, sizeof(int) * _ints.Length);
            _computeBuffer.SetData(_ints);
            _computeShader.SetBuffer(_kernel.ID, "Input", _computeBuffer);
            _temp = new List<int>(_ints);
        }

        private void Update()
        {
            _computeBuffer.SetData(_temp);

            for (int i = 0; i < _digits; ++i)
            {
                _computeShader.SetInt("Step", i);
                _kernel.Dispatch();   
            }
            
            _computeBuffer.GetData(_ints);
        }
    }
}