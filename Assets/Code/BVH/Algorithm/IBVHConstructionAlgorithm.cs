namespace Code.Components.MortonCodeAssignment
{
    public interface IBVHConstructionAlgorithm
    { 
        void Execute(int leavesCount);
        void Prepare();
        void Dispose() {}
    }
}