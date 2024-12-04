using EditorWrapper;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class NearestNeighbourVisualization : IDrawable
    {
        private readonly BVHNode[] _nodes;
        private readonly int _leavesCount;

        public NearestNeighbourVisualization(BVHNode[] nodes, int leavesCount)
        {
            _leavesCount = leavesCount;
            _nodes = nodes;
        }

        public void Draw()
        {
            for (int i = 0; i < _leavesCount; ++i)
            {
                uint nearestNeighbour = _nodes[i].X;
                Vector3 position = _nodes[i + _leavesCount].Box.Centroid;
                Vector3 neighbourPosition = _nodes[nearestNeighbour + _leavesCount].Box.Centroid;
                Vector3 direction = (neighbourPosition - position).normalized;
                Vector3 offset = Vector3.Cross(direction, Vector3.up) * 0.25f;
                Vector3 alongsideOffset = direction * 0.25f;
                
                position += offset + alongsideOffset;
                neighbourPosition += offset - alongsideOffset;
                
                Gizmos.DrawLine(position, neighbourPosition);
                GizmosUtils.SaveColor();
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(position, 0.1f);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(neighbourPosition, 0.1f);
                GizmosUtils.RestoreColor();
            }
        }
    }
}