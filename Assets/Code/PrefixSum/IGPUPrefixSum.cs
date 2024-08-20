using System;

namespace Code
{
    public interface IGPUPrefixSum : IDisposable
    {
        void Dispatch();
    }
}