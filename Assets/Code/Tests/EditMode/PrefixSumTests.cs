using Code;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

[TestFixture]
public class PrefixSumTests
{
    private ComputeShader _prefixSumShader;
    private int[] _sizes;
    private int _seed;

    [OneTimeSetUp]
    public void GlobalSetup()
    {
        _prefixSumShader = AssetDatabase.LoadAssetAtPath<ComputeShader>("Assets/Code/PrefixSum/PrefixSum.compute");
        _sizes = new[] { 1, 2, 3, 10, 83, 777, 999, 312, 7, 100, 1000, 10_000, 100_000, 1_000_000 };
        _seed = 0;
    }

    [Test]
    public void RunPrefixSumTests()
    {
        foreach (int size in _sizes)
        {
            PrefixSumTest test = new(size, _seed, _prefixSumShader);
            test.Run();
            test.Dispose();
        }
    }
}