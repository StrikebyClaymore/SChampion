using UnityEngine;
using UnityEngine.Events;

namespace Systems.Game.Entities
{
    public class Ball : MonoBehaviour
    {
        [field: SerializeField] public Rigidbody2D RB { get; private set; }
        [field: SerializeField] public float HalfSize { get; private set; } = 0.35f;
        public float InitialOffset;
        [SerializeField] private float _initForce;
        public readonly UnityEvent<Entity> OnTriggerEnter = new();

        public void Push()
        {
            RB.AddForce(Vector2.up * _initForce, ForceMode2D.Impulse);
        }

        public void MoveX(float newX)
        {
            RB.MovePosition(new Vector2(newX, RB.position.y));
        }
        
        public void MoveY(float newY)
        {
            RB.MovePosition(new Vector2(RB.position.x, newY));
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent<Entity>(out var entity))
            {
                OnTriggerEnter?.Invoke(entity);
            }
        }

        private void OnValidate()
        {
            RB = GetComponent<Rigidbody2D>();
        }
    }
}