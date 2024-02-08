using Code.View;
using UnityEngine;

public class Payload : MonoBehaviour
{
    [SerializeField] private IntersectingSpheresManager _manager;
    [SerializeField] [Min(1)] private int _x = 1;
    [SerializeField] [Min(1)] private int _y = 1;
    
    [ContextMenu("Create")]
    private void Rebuild()
    {
        for (int i = transform.childCount; i > 0; --i)
        {
            _manager.Remove(transform.GetChild(0).GetComponent<IntersectingSphere>());
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        for (int i = 0; i < _x; ++i)
        {
            for (int j = 0; j < _y; ++j)
            {
                GameObject newChild = new($"{i} {j}");
                IntersectingSphere sphere = newChild.AddComponent<IntersectingSphere>();
                _manager.Add(sphere);
                
                Vector3 randomColor = new Vector3(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
                Vector3 inversedColor = Vector3.one - randomColor;
                
                sphere.InjectOnChangedCallback(_manager);
                newChild.transform.parent = transform;
                newChild.transform.position = new Vector3(i, 0, j) * 1.5f;
                sphere.Color = new Color(randomColor.x, randomColor.y, randomColor.z, 1);
                sphere.IntersectionColor = new Color(inversedColor.x, inversedColor.y, inversedColor.z, 1);
            }
        }
        
        _manager.UpdateBuffer();
    }
}