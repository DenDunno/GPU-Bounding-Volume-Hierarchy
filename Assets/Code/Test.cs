using System;
using Code.Utils.ShaderUtils.Buffer;
using UnityEngine;

namespace Code
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private ComputeShader _prefixSumShader;
        [SerializeField] private ComputeShader _sortShader;
        [SerializeField] private int[] _input;
        [SerializeField] private int[] _output;
        private GPURadixSort _sort;

        private void OnValidate()
        {
            _output = new int[_input.Length];
        }

        private void Start()
        {
            ComputeBuffer buffer = new(_input.Length, sizeof(int));
            buffer.SetData(_input);
            
            IGPUPrefixSum prefixSum = new GPUPrefixSumFactory(_prefixSumShader, buffer).Create();
            prefixSum.Dispatch();
            
            buffer.GetData(_output);
        }
    }
}