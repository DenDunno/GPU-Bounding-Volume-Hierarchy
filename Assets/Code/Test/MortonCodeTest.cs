using Code;
using Code.Components.MortonCodeAssignment;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace
{
    public class MortonCodeTest : MonoBehaviour
    {
        [SerializeField] private MortonCode[] _input;
        [SerializeField] private MortonCode[] _output;

        [Button]
        private void Generate(int size = 5, int maxValue = 20)
        {
            _input = new MortonCode[size];

            for (int i = 0; i < size; ++i)
            {
                _input[i] = new MortonCode()
                {
                    Code = (uint)Random.Range(0, maxValue),
                    ObjectId = (uint)Random.Range(0, 30)
                };
            }
        }
        
        [Button]
        private void Sort()
        {
            _output = new MortonCode[_input.Length];
            ShadersPresenter shaders = new ShadersPresenter().Load();
            using GPURadixSort<MortonCode> sort = new(shaders.Sorting, shaders.PrefixSum, _input.Length);
            
            sort.SetData(_input);
            sort.Execute(_input.Length);
            sort.GetData(_output);
        }
    }
}