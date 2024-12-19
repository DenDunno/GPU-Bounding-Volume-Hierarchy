using EditorWrapper;

namespace DefaultNamespace.Code.GeometryGeneration.Plane
{
    public class PlaneDebug : IDrawable
    {
        private readonly CircleVisualization _circle = new();
        private readonly PlaneGenerationData _data;

        public PlaneDebug(PlaneGenerationData data)
        {
            _data = data;
        }

        public void Draw()
        {
            _circle.Draw(_data.Radius, _data.Point, _data.Normal);
        }
    }
}