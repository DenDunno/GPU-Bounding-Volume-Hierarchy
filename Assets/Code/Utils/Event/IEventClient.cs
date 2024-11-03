using System;

namespace Code.Components.MortonCodeAssignment.Event
{
    public interface IEventClient
    {
        void AddListener(Action listener);
        void RemoveListener(Action listener);
    }
}