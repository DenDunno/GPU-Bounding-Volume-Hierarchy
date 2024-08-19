using UnityEngine;

namespace Code
{
    public static class ExpectedPrefixSum
    {
        public static void CheckOutput(int[] input, int[] output)
        {
            int[] expectedPrefixSum = CreateExpectedPrefixSum(input);
            CheckIfEqual(expectedPrefixSum, output);
        }

        private static int[] CreateExpectedPrefixSum(int[] input)
        {
            int[] result = new int[input.Length];
            for (int i = 1; i < result.Length; ++i)
            {
                result[i] = input[i - 1] + result[i - 1];
            }

            return result;
        }

        private static void CheckIfEqual(int[] expectedPrefixSum, int[] output)
        {
            for (int i = 0; i < output.Length; ++i)
            {
                if (expectedPrefixSum[i] != output[i])
                {
                    Debug.LogError($"Error. I = {i}. {expectedPrefixSum[i]} != {output[i]}");
                    return;
                }
            }
            
            Debug.Log("Success");
        }
    }
}