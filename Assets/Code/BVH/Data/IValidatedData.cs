
namespace Code.Components.MortonCodeAssignment
{
    public interface IValidatedData
    {
        void OnValidate();
        bool IsValid();
    }
}