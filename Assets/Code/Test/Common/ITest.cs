namespace Code.Test
{
    public interface ITest
    {
        void Initialize() {}
        bool Run(int index, InputGenerationRules rules, int[] input);
    }
}