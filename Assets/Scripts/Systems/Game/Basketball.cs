using Systems.Game.Entities;
using UI.Components;
using UnityEngine;

namespace Systems.Game
{
    public class Basketball : GameBase
    {
        [SerializeField] private CustomPushButton _controlButton;
        [SerializeField] private float _jumpForce = 1f;

        protected override void Awake()
        {
            Physics2D.gravity = new Vector2(0, -9.8f);
            _controlButton.OnPressed.AddListener(MoveBalls);
        }

        private void MoveBalls()
        {
            if(_balls.Count == 0)
                return;
            var firstBall = _balls[0];
            firstBall.RB.AddForce(Vector2.up * _jumpForce);
            /*float targetY = firstBall.RB.position.y + _jumpForce;
            if (targetY > _edges.y)
                return;
            firstBall.MoveY(targetY);
            for (int i = 1; i < _balls.Count; i++)
            {
                var ball = _balls[i];
                ball.MoveY(targetY + ball.InitialOffset);
            }*/
        }
        
        protected override void SpawnBall(Entity entity = null)
        {
            if (entity == null)
            {
                SpawnBall(_ballStartPoint.position);
                return;
            }
            var value = entity.Value;
            var entityPosition = entity.transform.position;
            EntityDespawn(entity);
            var firstBall = _balls[0];
            var halfSize = firstBall.HalfSize;
            for (int i = 0; i < value; i++)
            {
                var spawnPosition = _ballStartPoint.position;
                spawnPosition.z = 0;
                if(_balls.Count > 0)
                    spawnPosition = new Vector3(spawnPosition.x + _balls.Count * halfSize, _balls[^1].transform.position.y, spawnPosition.z);
                float yOffset = Mathf.Min(halfSize, entityPosition.y - spawnPosition.y);
                spawnPosition.y += yOffset;
                var ball = SpawnBall(spawnPosition);
                ball.RB.isKinematic = true;
                ball.RB.constraints = RigidbodyConstraints2D.FreezeAll;
                ball.transform.SetParent(firstBall.transform);;
                ball.InitialOffset = ball.transform.position.y - _balls[0].transform.position.y;
            }
        }
    }
}