namespace Code.Utils.Provider
{
    public interface IProvider<out T>
    {
        T Provide();
    }
}