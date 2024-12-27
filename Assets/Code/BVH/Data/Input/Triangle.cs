using Code.Data;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public readonly struct Triangle : IAABBProvider
    {
        private readonly Vector3 _first;
        private readonly Vector3 _second;
        private readonly Vector3 _third;

        public Triangle(Vector3 first, Vector3 second, Vector3 third)
        {
            _first = first;
            _second = second;
            _third = third;
        }

        public AABB CalculateBox()
        {
            return new AABB(
                min: Vector3.Min(_first, Vector3.Min(_second, _third)),
                max: Vector3.Max(_first, Vector3.Max(_second, _third)));
        }

        public static Triangle operator *(Matrix4x4 matrix, Triangle triangle)
        {
            return new Triangle(matrix * triangle._first, matrix * triangle._second, matrix * triangle._third);
        }
    }
}