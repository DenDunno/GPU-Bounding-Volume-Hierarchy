using System;
using UnityEngine;

namespace Code.Utils.Extensions
{
    public readonly struct TransformSnapshot : IEquatable<TransformSnapshot>
    {
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;
        public readonly Vector3 Scale;

        public TransformSnapshot(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
        
        public bool Equals(TransformSnapshot other)
        {
            return Rotation.Equals(other.Rotation) && 
                   Position.Equals(other.Position) &&
                   Scale.Equals(other.Scale);
        }

        public override bool Equals(object obj)
        {
            return obj is TransformSnapshot other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Rotation, Position, Scale);
        }
    }
}