using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Test
{
    public class UpdateTest : MonoBehaviour
    {
        [SerializeField] private int _startIndex;
        [SerializeField] private int _endIndex;
        [SerializeField] private int _counter;
        [SerializeField] private int _minValue;
        [SerializeField] private int _maxValue;
        [SerializeField] private int _seed;
        
        [Button]
        private void Run()
        {
            ITest[] tests = GetComponents<ITest>();
            StartCoroutine(RunTests(tests));
        }
        
        [Button]
        private void Stop()
        {
            StopAllCoroutines();
        }

        private IEnumerator RunTests(ITest[] tests)
        {
            bool success = true;
            _counter = _startIndex;

            while (_counter++ < _endIndex && success)
            {
                RandomCollectionGeneration collectionGeneration = new(_seed, _counter, _minValue, _maxValue);
                success = tests.All(test => test.Run(collectionGeneration.Create()));
                yield return null;
            }

            ReportResult(success);
        }

        private void ReportResult(bool success)
        {
            string notification = success ? 
                "<color=green>Tests passed successfully</color>" : 
                "<color=red>Tests failed</color>";
        
            Debug.Log(notification);
        }
    }
}