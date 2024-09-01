using Sirenix.OdinInspector;
using UnityEngine;

namespace Code
{
    public class PrefixSumUpdateTest : MonoBehaviour
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

        public void Setup(int size)
        {
            _test?.Dispose();
            _test = new PrefixSumTest(size, _seed, _prefixSumShader);
        }

        private void Update()
        {
            if (_update)
            {
                RunTest();
            }
        }

        [Button]
        private void RunTest()
        {
            CollectionComparisonResult<int> result = _test.Run();

            if (result.IsEqual == false)
            {
                Debug.LogError($"Test failed. " +
                               $"Expected {result.FirstValue} " +
                               $"Actual = {result.SecondValue} " +
                               $"Index = {result.Index}");
            }
        }

        private void OnDestroy()
        {
            _test.Dispose();
        }
    }
}