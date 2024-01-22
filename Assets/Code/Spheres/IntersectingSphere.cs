using UnityEngine;

[ExecuteAlways]
public class IntersectingSphere : MonoBehaviour
{
    private int _index = -1;
    public int Index => _index;

    private void OnValidate()
    {
    }
}