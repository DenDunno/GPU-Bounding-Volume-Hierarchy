using UnityEngine;

namespace Code
{
    public class GPURadixSortInput
    {
        public readonly ComputeShader PrefixSumShader;
        public readonly ComputeShader SortShader;
        public readonly int SortedBitsPerPass;
        public readonly int ArraySize;

        public GPURadixSortInput(ComputeShader sortShader, ComputeShader prefixSumShader, int arraySize, int sortedBitsPerPass = 2)
        {
            SortedBitsPerPass = sortedBitsPerPass;
            PrefixSumShader = prefixSumShader;
            SortShader = sortShader;
            ArraySize = arraySize;
        }

        public int BlockSize => AllPossibleValues;
        public Vector3Int PayloadDispatch => new(ArraySize, BlockSize, 1);
        private int AllPossibleValues => (int)Mathf.Pow(2, SortedBitsPerPass);
    }
}