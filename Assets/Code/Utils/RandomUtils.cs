
using UnityEngine;

namespace Code
{
    public static class RandomUtils
    {
        public static Color GenerateBrightColor()
        {
            return Color.HSVToRGB(Random.Range(0, 1f), Random.Range(0, 1f), 1);
        }
    }
}