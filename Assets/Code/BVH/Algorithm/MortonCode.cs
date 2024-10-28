using System;

namespace DefaultNamespace
{
    [Serializable]
    struct MortonCode
    {
        public uint ObjectId;
        public uint Code;

        public static int GetSize()
        {
            return 4 + 4;
        }

        public override string ToString()
        {
            return "MortonCode[ObjectId=" + ObjectId + ", Code=" + Code + "]";
        }
    }
}