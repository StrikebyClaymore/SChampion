using UnityEngine;
using UnityEngine.Events;

namespace Systems.Game.Entities
{
    public class DespawnArea : MonoBehaviour
    {
        public readonly UnityEvent<Entity> OnTriggerEnter = new();
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent<Entity>(out var entity))
            {
                OnTriggerEnter?.Invoke(entity);
            }
        }
    }
}