using UnityEngine;

namespace Code
{
    public class GizmosUtils
    {
        private static Color _savedColor;

        public static void SaveColor()
        {
            _savedColor = Gizmos.color;
        }
        
        public static void SetColor(Color color)
        {
            _savedColor = Gizmos.color;
            Gizmos.color = color;
        }

        public static void RestoreColor()
        {
            Gizmos.color = _savedColor;
        }
    }
}