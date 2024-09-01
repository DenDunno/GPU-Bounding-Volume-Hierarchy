using Code.Utils.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Test
{
    public abstract class CollectionComparisonTest : MonoBehaviour, ITest
    {
        [Button]
        public bool Run(int index, InputGenerationRules rules, int[] input)
        {
            CollectionComparisonResult<int> result = RunComparisonTest(index, rules, input);
            
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
        protected abstract CollectionComparisonResult<int> RunComparisonTest(int index, InputGenerationRules rules, int[] input);
    }
}