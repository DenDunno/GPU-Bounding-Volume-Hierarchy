using UnityEngine;

namespace Code
{
    public class GPURadixSortInput
    {
        public readonly int SortedBitsPerPass;
        public readonly int ArraySize;

        public GPURadixSortInput(int arraySize, int sortedBitsPerPass = 2)
        {
            SortedBitsPerPass = sortedBitsPerPass;
            ArraySize = arraySize;
        }

        public int BlockSize => AllPossibleValues;
        public Vector3Int PayloadDispatch => new(ArraySize, BlockSize, 1);
        private int AllPossibleValues => (int)Mathf.Pow(2, SortedBitsPerPass);
    }
}