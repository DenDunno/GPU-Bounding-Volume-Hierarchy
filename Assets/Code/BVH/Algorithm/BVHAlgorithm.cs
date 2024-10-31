using System;
using Code.Utils.Extensions;
using DefaultNamespace;
using Random = UnityEngine.Random;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHAlgorithm : IDisposable
    {
        private readonly GPURadixSort<MortonCode> _mortonCodesSorting;
        private readonly HPLOC _bvhConstruction;
        private readonly SetupStage _setupStage;
        public readonly BVHBuffers Buffers;

        public BVHAlgorithm(BVHShaders bvhShaders, int bufferSize)
        {
            Buffers = new BVHBuffers(bufferSize);
            _setupStage = new SetupStage(bvhShaders.Setup, Buffers);
            _bvhConstruction = new HPLOC(bvhShaders.BVHConstruction, Buffers);
            _mortonCodesSorting = new GPURadixSort<MortonCode>(bvhShaders.Sorting, bvhShaders.PrefixSum, bufferSize);
        }

        public void Initialize()
        {
            _mortonCodesSorting.SetData(Buffers.MortonCodes);
            _bvhConstruction.Prepare();
            _setupStage.Prepare();
        }

        public void Execute(int count)
        {
            _setupStage.Execute(count);
            Buffers.MortonCodes.Print<MortonCode>("", x => $"{x.Code} ");
            //_mortonCodesSorting.Execute(count);

            // Random.InitState(Environment.TickCount);
            // MortonCode[] mortonCodes = new MortonCode[8];
            // for (int i = 3; i < mortonCodes.Length; ++i)
            // {
            //     mortonCodes[i].ObjectId = 0;
            //     mortonCodes[i].Code = mortonCodes[i - 1].Code + (uint)Random.Range(0, 1000);
            // }
            
            // mortonCodes.Print();
            // Buffers.MortonCodes.SetData(mortonCodes);
            //_bvhConstruction.Execute(count);
        }

        public void Dispose()
        {
            Buffers.Dispose();
            _mortonCodesSorting.Dispose();
        }
    }
}