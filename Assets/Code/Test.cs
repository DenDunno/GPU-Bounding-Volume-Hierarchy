using Code.Utils.Extensions;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Code
{
    [ExecuteInEditMode]
    public class Test : MonoBehaviour
    {
        [SerializeField] private ComputeShader _prefixSumShader;
        [SerializeField] private ComputeShader _sortShader;
        [SerializeField] private int _seed = 0;
        [SerializeField] private bool _success;
        [SerializeField] private bool _update;
        [SerializeField] private bool _showOutput;
        [SerializeField] [Range(0, 100_000)] private int _updateSize = 10;
        [SerializeField] private int[] _sizes;
        [SerializeField] private int[] _input;
        [SerializeField] private int[] _output;
        private ExpectedPrefixSum _expectedPrefixSum;
        private IGPUPrefixSum _prefixSum;
        private ComputeBuffer _buffer;
        private GPURadixSort _sort;

        private void Initialize(int size)
        {
            Dispose();
            _input = new RandomCollectionGeneration(_seed, size, 0, 10).Create();
            _buffer = new ComputeBuffer(_input.Length, sizeof(int));
            _output = new int[size];
            _prefixSum = new GPUPrefixSumFactory(_prefixSumShader, _buffer).Create();
            _expectedPrefixSum = new ExpectedPrefixSum(_input.Length, _success);
            _expectedPrefixSum.Initialize(_input);   
        }

        #if  UNITY_EDITOR
        private void OnEnable()
        {
            AssemblyReloadEvents.beforeAssemblyReload += Dispose;
        }

        private void OnDisable()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= Dispose;
        }
        #endif

        private void Update()
        {
            if (_update)
            {
                RunTest(_updateSize);
            }
        }

        [Button]
        private void RunTests()
        {
            foreach (int size in _sizes)
            {
                RunTest(size);
            }
        }

        private void RunTest(int size)
        {
            Initialize(size);
            _buffer.SetData(_input);
            _prefixSum.Dispatch();
            _buffer.GetData(_output);
            _expectedPrefixSum.CheckOutput(_output);

            if (_showOutput)
            {
                _output.Print("Output = ");
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Dispose()
        {
            _buffer?.Dispose();
            _prefixSum?.Dispose();
        }
    }
}