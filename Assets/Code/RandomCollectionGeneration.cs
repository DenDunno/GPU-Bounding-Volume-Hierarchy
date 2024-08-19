using UnityEngine;

namespace Code
{
    public class RandomCollectionGeneration
    {
        private readonly int _maxValue;
        private readonly int _seed;
        private readonly int _size;

        public RandomCollectionGeneration(int seed, int size, int maxValue)
        {
            _maxValue = maxValue;
            _seed = seed;
            _size = size;
        }

        public int[] Create()
        {
            int[] randomCollection = new int[_size];
            Random.InitState(_seed);
            
            for (int i = 0; i < randomCollection.Length; ++i)
            {
                randomCollection[i] = Random.Range(0, _maxValue);
            }

            return randomCollection;
        }
    }
}