using UnityEngine;

namespace Code.Utils.Extensions
{
    public static class TransformExtensions
    {
        public static TransformSnapshot TakeSnapshot(this Transform transform)
        {
            return new TransformSnapshot(transform.position, transform.rotation, transform.lossyScale);
        }
        
        public static bool HasChanged(this Transform transform, ref TransformSnapshot previousSnapshot)
        {
            TransformSnapshot snapshot = transform.TakeSnapshot();
            bool hasChanged = snapshot.Equals(previousSnapshot) == false;

            if (hasChanged)
            {
                previousSnapshot = snapshot;    
            }
            
            return hasChanged;
        }
    }
}