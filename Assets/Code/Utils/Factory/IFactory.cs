namespace Code.Utils.Factory
{
    public interface IFactory<out T>
    {
        T Create();
    }
}