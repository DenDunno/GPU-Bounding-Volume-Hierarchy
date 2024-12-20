using System.Collections;
using System.Text;
using Code.Components.MortonCodeAssignment;
using Sirenix.OdinInspector;
using UnityEngine;

public class BVHTest : MonoBehaviour
{
    private string _test;
    
    [Button]
    private void LaunchIterations(int iterations = 100)
    {
        StopAllCoroutines();
        StartCoroutine(RunIterations(iterations));
    }

    private IEnumerator RunIterations(int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            GetComponent<StaticBVH>().Bake();
            yield return null;
        }
    }

    [Button]
    private void Launch()
    {
        StopAllCoroutines();
        StartCoroutine(RunTest());
    }

    private IEnumerator RunTest()
    {
        StaticBVH bvh = GetComponent<StaticBVH>();
        StringBuilder stringBuilder = new();
        
        while (true)
        {
            stringBuilder.Clear();
            bvh.Bake();
            BVHNode[] tree = bvh.GPUBridge.FetchTree();
            
            foreach (BVHNode node in tree)
            {
                stringBuilder.AppendLine($"{node}\n");
            }

            if (stringBuilder.ToString() != _test)
            {
                _test = stringBuilder.ToString();
                break;
            }

            yield return null;
        }
    }
}