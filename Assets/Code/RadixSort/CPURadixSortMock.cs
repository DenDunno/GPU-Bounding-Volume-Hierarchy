using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code
{
    public class CPURadixSortMock
    {
        public void Sort(int[] input, int[] output, int threads)
        {
            int threadGroups = Mathf.CeilToInt((float)input.Length / threads);

            for (int groupId = 0; groupId < threadGroups; ++groupId)
            {
                for (int threadId = 0; threadId < threads; ++threadId)
                {
                    int globalId = groupId * threads + threadId;

                    if (globalId < input.Length)
                    {
                        HandleInput(input, output, globalId, threadId, groupId);   
                    }
                }
            }
        }

        private void HandleInput(int[] input, int[] output, int globalId, int threadId, int groupId)
        {
            globalId = Mathf.Min(input.Length - 1, globalId);
            output[globalId] = threadId;
        }
    }
}