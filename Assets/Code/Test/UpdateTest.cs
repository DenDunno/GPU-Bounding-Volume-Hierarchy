using System;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Test
{
    public class UpdateTest : MonoBehaviour
    {
        [SerializeField] private int _startIndex;
        [SerializeField] private int _endIndex;
        [SerializeField] private float _coolDown;
        [SerializeField] private int _counter;
        [SerializeField] private InputGenerationRules _rules;
        [SerializeField] private int[] _input;

        [Button]
        private async void Run()
        {
            ITest[] tests = GetComponents<ITest>();
            bool success = await RunTests(tests);
            ReportResult(success);
        }

        private async Task<bool> RunTests(ITest[] tests)
        {
            bool success = true;
            _counter = _startIndex;

            while (_startIndex < _endIndex && success)
            {
                success = tests.All(test => test.Run(_counter++, _rules, _input));

                await Task.Delay(TimeSpan.FromSeconds(_coolDown));
            }

            return success;
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