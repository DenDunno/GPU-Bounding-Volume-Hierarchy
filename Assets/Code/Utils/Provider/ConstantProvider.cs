namespace Code.Utils.Provider
{
    public class ConstantProvider<T> : IProvider<T>
    {
        private readonly T _output;

        public ConstantProvider(T output)
        {
            _output = output;
        }

        public T Provide()
        {
            return _output;
        }
    }
}