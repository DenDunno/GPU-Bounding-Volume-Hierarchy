using Code.Utils.Extensions;
using UnityEngine;


public class RaceConditionTest : MonoBehaviour
{
    [SerializeField] private ComputeShader _shader;
    private ComputeBuffer _firstInput;
    private ComputeBuffer _secondInput;
    private ComputeBuffer _output;

    private void Start()
    {
        _firstInput = new ComputeBuffer(1, sizeof(int));
        _secondInput = new ComputeBuffer(1, sizeof(int));
        _output = new ComputeBuffer(1, sizeof(int));
        _firstInput.SetData(new[] { 1 });
        _secondInput.SetData(new[] { 2 });
        _shader.SetBuffer(0, "Output", _output);
    }

    private void Update()
    {
        _shader.SetBuffer(0, "Input", _firstInput);
        _shader.Dispatch(0, 1, 1, 1);

        _shader.SetBuffer(0, "Input", _secondInput);
        _shader.Dispatch(0, 1, 1, 1);

        _output.PrintInt("Output = ");
    }
}