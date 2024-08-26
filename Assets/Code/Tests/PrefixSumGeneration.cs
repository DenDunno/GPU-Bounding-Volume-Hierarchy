namespace Code
{
    public class PrefixSumGeneration
    {
        public int[] Generate(int[] input)
        {
            int[] prefixSum = new int[input.Length];
            
            for (int i = 1; i < prefixSum.Length; ++i)
            {
                prefixSum[i] = input[i - 1] + prefixSum[i - 1];
            }

            return prefixSum;
        }
    }
}