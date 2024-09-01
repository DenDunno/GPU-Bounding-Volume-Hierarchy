using System;
using Code.Utils.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Test
{
    public abstract class CollectionComparisonTest : MonoBehaviour, ITest
    {
        [SerializeField] protected int[] Input;
        [SerializeField] protected int[] Output;

        private void OnValidate()
        {
            Output = new int[Input.Length];
        }

        [Button]
        public bool RunInput()
        {
            return Run(Input);
        }
        
        [Button]
        public bool RunRandom(int size, int maxValue)
        {
            Input = new RandomCollectionGeneration(0, size, 0, maxValue).Create();
            return Run(Input);
        }
        
        public bool Run(int[] input)
        {
            CollectionComparisonResult<int> result = RunComparisonTest(input);
            
            if (result.IsEqual == false)
            {
                Debug.LogError($"{TestName} test failed. " +
                               $"Expected {result.FirstValue} " +
                               $"Actual = {result.SecondValue} " +
                               $"Index = {result.Index}");

                result.FirstCollection.Print(string.Empty);
                result.SecondCollection.Print(string.Empty);
            }

            return result.IsEqual;
        }

        protected abstract string TestName { get; }
        protected abstract CollectionComparisonResult<int> RunComparisonTest(int[] input);
    }
}