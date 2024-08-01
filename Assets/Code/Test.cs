using System;
using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private ComputeShader _computeShader;
        [SerializeField] private int[] _ints;
        [SerializeField] private int[] _debug;
        [SerializeField] [Range(0, 32)] private int _digits = 1;
        private ComputeBuffer _input;
        private Kernel _kernel;
        private int[] _temp;

        private void Start()
        {
            _kernel = new Kernel(_computeShader, "PrefixSum", new Vector3Int(_ints.Length, 1, 1));
            _input = new ComputeBuffer(_ints.Length, sizeof(int));
            _temp = new int[_ints.Length];
            Array.Copy(_ints, _temp, _temp.Length);
            
            _computeShader.SetBuffer(_kernel.ID, "Input", _input);
        }

        private void Update()
        {
            _input.SetData(_temp);

            for (int i = 0; i < _digits; ++i)
            {
                _computeShader.SetInt("BitOffset", i);
                _kernel.Dispatch();   
            }
            
            _input.GetData(_ints);
        }
    }
}