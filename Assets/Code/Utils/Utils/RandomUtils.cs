using UnityEngine;

namespace Code
{
    public static class RandomUtils
    {
        private static Random.State _state;
        
        public static void InitState(int seed)
        {
            _state = Random.state;
            Random.InitState(seed);
        }

        public static void RestoreState()
        {
            Random.state = _state;
        }
        
        public static Color GenerateBrightColor()
        {
            return Color.HSVToRGB(Random.Range(0, 1f), Random.Range(0, 1f), 1);
        }
    }
}