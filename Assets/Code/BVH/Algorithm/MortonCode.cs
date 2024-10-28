using System;

namespace DefaultNamespace
{
    [Serializable]
    struct MortonCode
    {
        public uint ObjectId;
        public uint Code;

        public MortonCode(uint objectId, uint code)
        {
            ObjectId = objectId;
            Code = code;
        }

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