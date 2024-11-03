using System;

namespace Code.Components.MortonCodeAssignment.Event
{
    public class EventWrapper : IEventClient, IEventPublisher
    {
        private event Action Value;

        public void AddListener(Action listener)
        {
            Value += listener;
        }

        public void RemoveListener(Action listener)
        {
            Value -= listener;
        }

        public void Raise()
        {
            Value?.Invoke();
        }
    }
}