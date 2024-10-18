using UnityEngine;
using UnityEngine.Events;

namespace Systems
{
    public class MonoTimer : MonoBehaviour
    {
        public float Time;
        public float TimeLeft;
        public readonly UnityEvent<float> OnTimeTick = new();
        public readonly UnityEvent OnCompleted = new();

        public void Enable()
        {
            TimeLeft = Time;
            enabled = true;
        }

        private void Update()
        {
            if(!enabled)
                return;
            TimeLeft -= UnityEngine.Time.deltaTime;
            OnTimeTick?.Invoke(TimeLeft);
            if (TimeLeft <= 0)
            {
                OnCompleted?.Invoke();
                enabled = false;
            }
        }
    }
}