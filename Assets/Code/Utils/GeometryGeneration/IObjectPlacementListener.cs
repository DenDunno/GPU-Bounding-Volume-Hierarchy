using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Code.GeometryGeneration
{
    public interface IObjectPlacementListener
    {
        void Accept(IReadOnlyList<GameObject> objects);
    }
}