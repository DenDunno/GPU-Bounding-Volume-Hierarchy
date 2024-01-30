using UnityEngine;

namespace Code.Utils
{
    public static class ColorExtensions
    {
        public static Color GetRandom(int seed = 0)
        {
            Random.InitState(seed);
            Vector3 randomColor = new Vector3(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
            return new Color(randomColor.x, randomColor.y, randomColor.z, 1);
        }
        
        public static Color InverseWithoutAlpha(this Color color)
        {
            return new Color(1 - color.r, 1 - color.g, 1 - color.b, 1);
        }
    }
}