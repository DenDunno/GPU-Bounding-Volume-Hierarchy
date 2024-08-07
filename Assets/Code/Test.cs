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
            _sort = new GPURadixSort(_computeShader, _input.Length);
            _sort.Initialize(new SetArrayOperation<int>(_input));
        }

        private void Update()
        {
            _sort.Execute(_output);
        }
    }
}