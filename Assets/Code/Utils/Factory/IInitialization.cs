
namespace Code.Utils.Factory
{
    public interface IInitialization<in T>
    {
        public void Initialize(T target);
    }
}