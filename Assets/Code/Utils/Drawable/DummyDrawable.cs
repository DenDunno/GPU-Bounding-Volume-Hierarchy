
namespace EditorWrapper
{
    public class DummyDrawable : IDrawable
    {
        public static readonly IDrawable Instance = new DummyDrawable();
        
        public void Draw()
        {
        }
    }
}