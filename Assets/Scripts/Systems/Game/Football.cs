using UnityEngine;
using UnityEngine.UI;

namespace Systems.Game
{
    public class Football : GameBase
    {
        [SerializeField] private Slider _controlSlider;

        protected override void Awake()
        {
            base.Awake();
            Physics2D.gravity = new Vector2(0, 0);
            _controlSlider.onValueChanged.AddListener(MoveBalls);
        }

        public override void StartGame(LevelData data)
        {
            _controlSlider.value = _controlSlider.maxValue / 2;
            base.StartGame(data);
        }
        
        public override void StartInfinityGame()
        {
            _controlSlider.value = _controlSlider.maxValue / 2;
            base.StartInfinityGame();
        }

        private void MoveBalls(float value)
        {
            if(_balls.Count == 0)
                return;
            float targetX = Mathf.Lerp(_edges.x, _edges.y, value);
            var firstBall = _balls[0];
            firstBall.MoveX(targetX);
            for (int i = 1; i < _balls.Count; i++)
            {
                var ball = _balls[i];
                ball.MoveX(targetX + ball.InitialOffset);
            }
        }
    }
}