using UnityEngine;

namespace Code
{
    public class GPURadixSortInput
    {
        public readonly ComputeShader PrefixSumShader;
        public readonly int SortedBitsPerPass = 2;
        public readonly ComputeShader SortShader;
        public readonly int ArraySize;

        public GPURadixSortInput(ComputeShader sortShader, ComputeShader prefixSumShader, int arraySize)
        {
            PrefixSumShader = prefixSumShader;
            SortShader = sortShader;
            ArraySize = arraySize;
        }

        public int Blocks => AllPossibleValues;
        public Vector3Int PayloadDispatch => new(ArraySize, Blocks, 1);
        private int AllPossibleValues => (int)Mathf.Pow(2, SortedBitsPerPass);
    }
}