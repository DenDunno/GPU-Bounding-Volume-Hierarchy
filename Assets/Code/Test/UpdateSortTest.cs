using System;
using Code;
using UnityEngine;

public class UpdateSortTest : MonoBehaviour
{
    [SerializeField] private int _maxValue;
    [SerializeField] private int _size;
    [SerializeField] private int _seed;
    [SerializeField] private ComputeShader _prefixSumShader;
    [SerializeField] private ComputeShader _sortShader;
    private int[] _randomCollection;
    private int[] _collectionToSort;
    private GPURadixSort<int> _gpuSort;

    private void Start()
    {
        Setup(_size);
    }

    public void Setup(int size)
    {
        _gpuSort?.Dispose();
        _randomCollection = new RandomCollectionGeneration(_seed, size, 0, _maxValue).Create();
        _gpuSort = new GPURadixSort<int>(_sortShader, _prefixSumShader, size);
        _collectionToSort = new int[size];
    }

    private void Update()
    {
        Array.Copy(_randomCollection, _collectionToSort, _collectionToSort.Length);
        _gpuSort.SetData(_collectionToSort);
        _gpuSort.Execute(_collectionToSort.Length);
    }

    private void OnDestroy()
    {
        _gpuSort.Dispose();
    }
}