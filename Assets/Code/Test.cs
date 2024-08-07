using System;
using Code.Utils.ShaderUtils.Buffer;
using UnityEngine;

namespace Code
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private ComputeShader _computeShader;
        [SerializeField] private int[] _input;
        [SerializeField] private int[] _output;
        private GPURadixSort _sort;

        private void OnValidate()
        {
            _output = new int[_input.Length];
        }

        private void Start()
        {
            _sort = new GPURadixSort(_input.Length, _computeShader);
            _sort.Initialize(new SetArrayOperation<int>(_input));
        }

        private void Update()
        {
            int[] sortedOutput = _sort.Execute();
            Array.Copy(sortedOutput, _output, _output.Length);
        }
    }
}