
namespace Code.Components.MortonCodeAssignment
{
    public class SurfaceAreaHeuristic
    {
        private readonly BVHNode[] _nodes;

        public SurfaceAreaHeuristic(BVHNode[] nodes)
        {
            _nodes = nodes;
        }

        public float Compute()
        {
            float total = 0;
            
            for (int i = 0; i < _nodes.Length; ++i)
            {
                total += _nodes[i].ComputeSurfaceArea();
            }

            return total;
        }
    }
}