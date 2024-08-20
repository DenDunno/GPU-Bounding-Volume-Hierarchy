using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Code
{
    public class ExpectedPrefixSum
    {
        private readonly int[] _expectedPrefixSum;
        private readonly bool _success;

        public ExpectedPrefixSum(int size, bool success)
        {
            _expectedPrefixSum = new int[size];
            _success = success;
        }

        public void Initialize(int[] input)
        {
            for (int i = 1; i < _expectedPrefixSum.Length; ++i)
            {
                _expectedPrefixSum[i] = input[i - 1] + _expectedPrefixSum[i - 1];
            }
        }

        public void CheckOutput(int[] output)
        {
            for (int i = 0; i < output.Length; ++i)
            {
                if (_expectedPrefixSum[i] != output[i])
                {
                    Debug.LogError($"Error at i = {i} Size = {_expectedPrefixSum.Length}. Expected = {_expectedPrefixSum[i]}, output = {output[i]}");
                    Debugger.Break();
                    return;
                }
            }

            if (_success)
            {
                Debug.Log("Success");
            }
        }
    }
}