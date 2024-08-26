using System;
using UnityEngine;

namespace Code
{
    public class UpdateTest : MonoBehaviour
    {
        [SerializeField] private ComputeShader _prefixSumShader;
        [SerializeField] private bool _update;
        [SerializeField] private int _size;
        [SerializeField] private int _seed;
        private PrefixSumTest _test;

        private void OnValidate()
        {
            Setup(_size);
        }

        private void Start()
        {
            Setup(_size);
        }

        private void Setup(int size)
        {
            _test?.Dispose();
            _test = new PrefixSumTest(size, _seed, _prefixSumShader);
        }

        private void Update()
        {
            if (_update)
            {
                _test.Run();
            }
        }

        private void OnDestroy()
        {
            _test.Dispose();
        }
    }
}