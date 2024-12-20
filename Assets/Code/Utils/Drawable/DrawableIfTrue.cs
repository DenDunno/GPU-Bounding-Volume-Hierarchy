using System;

namespace EditorWrapper
{ 
    public class DrawableIfTrue : IDrawable
    {
        private readonly IDrawable _conditionalDrawable;
        
        public DrawableIfTrue(IDrawable drawable, bool draw) : this(drawable, () => draw)
        {
        }
        
        public DrawableIfTrue(IDrawable drawable, Func<bool> drawCondition)
        {
            _conditionalDrawable = new ConditionalDraw(drawable, DummyDrawable.Instance, drawCondition);
        }
        
        public void Draw()
        {
            _conditionalDrawable.Draw();
        }
    }
}