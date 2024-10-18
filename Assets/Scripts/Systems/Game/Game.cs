using System;
using System.Collections.Generic;
using Extensions;
using Systems.Game.Entities;
using TMPro;
using UI.Components;
using Unity.VisualScripting;
using UnityEngine;

namespace Systems.Game
{
    public abstract class GameBase : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI BalanceText { get; private set; }
        [field: SerializeField] public CustomPushButton PauseButton { get; private set; }
        [SerializeField] private GameObject _timerPanel;
        [SerializeField] private TextMeshProUGUI _timerText;
        [field: SerializeField] public EGameTypes Type { get; private set; }
        [SerializeField] protected Transform _entitiesContainer;
        [SerializeField] protected Transform _ballStartPoint;
        [SerializeField] protected DespawnArea _despawnArea;
        [SerializeField] protected Vector2 _edges;
        [SerializeField] private Vector3 _obstacleStartPosition;
        [SerializeField] private Vector3 _finishSpawnPosition;
        [SerializeField] private Vector3 _obstacleStep;
        [SerializeField] private Ball _ballPrefab;
        [SerializeField] private Entity _obstaclePrefab;
        [SerializeField] private Entity _bonusPrefab;
        [SerializeField] private Entity _starPrefab;
        [SerializeField] private Entity _finishPrefab;
        [SerializeField] private float _startSpeed = 200f;
        [SerializeField] private Vector3 _startDirection;
        [SerializeField] protected float _acceleration = 0.1f;
        protected float _speed;
        private LevelData _levelData;
        protected int _mode;
        protected List<Ball> _balls = new List<Ball>();
        protected List<Entity> _entities = new List<Entity>();
        private List<Ball> _ballsPool = new List<Ball>();
        private List<Entity> _entitiesPool = new List<Entity>();
        private Entity _finish;
        public float Timer { get; set; }
        protected int _lastSeconds = 0;
        protected float _timeLeft;
        private int _wave = -1;
        private bool _levelCompleted;
        protected bool _gameStarted;
        public bool NeedUpdate;

        protected virtual void Awake()
        {
            _despawnArea.OnTriggerEnter.AddListener(EntityDespawn);
        }

        public virtual void CustomUpdate()
        {
            if(!_gameStarted)
                return;
            _speed += Time.deltaTime * _acceleration;
            if (_mode == 1)
            {
                Timer += Time.deltaTime;
                int currentSeconds = Mathf.FloorToInt(Timer);
                if (currentSeconds != _lastSeconds)
                {
                    _lastSeconds = currentSeconds;
                    SetTimer();
                }
            }
            var count = _entities.Count;
            for (int i = 0; i < count; i++)
            {
                var rect = _entities[i].Rect;
                Vector3 position = rect.anchoredPosition;
                position += _startDirection * (_speed * Time.deltaTime);
                rect.anchoredPosition = position;
                if(_entities.Count != count)
                    break;
            }
            if(_timeLeft <= 0)
                return;
            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0)
            {
                SpawnWave();
            }
        }

        public virtual void StartGame(LevelData data)
        {
            _levelData = data;
            _mode = 0;
            _wave = 0;
            _speed = _startSpeed;
            _levelCompleted = false;
            _timeLeft = _levelData.Waves[_wave].Delay;
            _timerPanel.Hide();
            SpawnBall();
            _gameStarted = true;
            NeedUpdate = true;
            if(_timeLeft == 0)
                SpawnWave();
        }

        public virtual void StartInfinityGame()
        {
            _mode = 1;
            _wave = 0;
            Timer = 0;
            _lastSeconds = 0;
            _speed = _startSpeed;
            _levelCompleted = false;
            _levelData = LevelGenerator.Generate(_levelData);
            _timeLeft = _levelData.Waves[_wave].Delay;
            _timerPanel.Show();
            SetTimer();
            SpawnBall();
            _gameStarted = true;
            NeedUpdate = true;
            if(_timeLeft == 0)
                SpawnWave();
        }
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
            NeedUpdate = _gameStarted;
            foreach (var ball in _balls)
                ball.gameObject.Show();
        }
        
        public virtual void Hide()
        {
            gameObject.SetActive(false);
            NeedUpdate = false;
            foreach (var ball in _balls)
                ball.gameObject.Hide();
        }

        public void Reset()
        {
            Timer = 0;
            _lastSeconds = 0;
            SetTimer();
        }

        private void SetTimer()
        {
            var timeFormatted = $"{Mathf.FloorToInt((float)_lastSeconds/60)} : {_lastSeconds:00}";
            _timerText.SetText(timeFormatted);
        }
        
        private void BallCollide(Entity entity)
        {
            var ball = _balls[^1];
            switch (entity.Type)
            {
                case EEntities.Obstacle:
                    ObstacleSubtractValue(ball, entity);
                    break;
                case EEntities.Bonus:
                    SpawnBall(entity);
                    break;
                case EEntities.Star:
                    PlayerData.AddIntValue(GameConstants.MONEY_SAVE_KEY, entity.Value);
                    GameEvents.InvokeMoneyChanged(PlayerData.GetInt(GameConstants.MONEY_SAVE_KEY));
                    EntityDespawn(entity);
                    break;
                case EEntities.Finish:
                    WinGame();
                    break;
            }
        }
        
        protected virtual void SpawnBall(Entity entity = null)
        {
            if (entity == null)
            {
                SpawnBall(_ballStartPoint.position);
                return;
            }
            var value = entity.Value;
            var entityPosition = entity.transform.position;
            EntityDespawn(entity);
            var halfSize = _balls[0].HalfSize;
            for (int i = 0; i < value; i++)
            {
                var spawnPosition = _ballStartPoint.position;
                spawnPosition.z = 0;
                if(_balls.Count > 0)
                    spawnPosition = new Vector3(_balls[^1].transform.position.x, spawnPosition.y + _balls.Count * halfSize, spawnPosition.z);
                float xOffset = Mathf.Min(halfSize, entityPosition.x - spawnPosition.x);
                spawnPosition.x += xOffset;
                var ball = SpawnBall(spawnPosition);
                ball.InitialOffset = ball.transform.position.x - _balls[0].transform.position.x;
            }
        }

        protected Ball SpawnBall(Vector3 spawnPosition)
        {
            Ball ball = null;
            foreach (var b in _ballsPool)
            {
                ball = b;
                ball.gameObject.SetActive(true);
                _ballsPool.Remove(b);
                break;
            }
            if(ball == null)
                ball = Instantiate(_ballPrefab);
            spawnPosition.z = 0;
            ball.transform.position = spawnPosition;
            ball.OnTriggerEnter.AddListener(BallCollide);
            ball.RB.isKinematic = false;
            ball.RB.constraints = RigidbodyConstraints2D.FreezeRotation;
            _balls.Add(ball);
            ball.Push();
            return ball;
        }

        private void BallDespawn(Ball ball)
        {
            ball.RB.isKinematic = true;
            ball.transform.SetParent(null);
            ball.OnTriggerEnter.RemoveListener(BallCollide);
            ball.gameObject.SetActive(false);
            _balls.Remove(ball);
            _ballsPool.Add(ball);
        }

        private Entity SpawnEntity(EEntities type)
        {
            Entity entity = null;
            for (int i = 0; i < _entitiesPool.Count; i++)
            {
                var entityInPool = _entitiesPool[i];
                if (entityInPool.Type == type && !_entities.Contains(entityInPool))
                {
                    entity = entityInPool;
                    entity.gameObject.SetActive(true);
                    _entitiesPool.Remove(entity);
                    _entities.Add(entity);
                    return entity;
                }
            }
            switch (type)
            {
                case EEntities.Obstacle:
                    entity = Instantiate(_obstaclePrefab, _entitiesContainer);
                    break;
                case EEntities.Bonus:
                    entity = Instantiate(_bonusPrefab, _entitiesContainer);
                    break;
                case EEntities.Star:
                    entity = Instantiate(_starPrefab, _entitiesContainer);
                    break;
            }
            _entities.Add(entity);
            return entity;
        }

        protected void EntityDespawn(Entity entity)
        {
            entity.Release();
            _entities.Remove(entity);
            _entitiesPool.Add(entity);
        }

        private void DespawnAll()
        {
            foreach (var entity in _entities)
            {
                entity.Rect.anchoredPosition = Vector2.zero;
                entity.gameObject.SetActive(false);
                _entitiesPool.Add(entity);
            }
            _entities.Clear();
            foreach (var ball in _balls)
            {
                ball.transform.SetParent(null);
                ball.RB.isKinematic = true;
                ball.transform.position = Vector3.down * 1000;
                ball.OnTriggerEnter.RemoveListener(BallCollide);
                ball.gameObject.SetActive(false);
                _ballsPool.Add(ball);
            }
            _balls.Clear();
        }
        
        private void ObstacleSubtractValue(Ball ball, Entity obstacle)
        {
            obstacle.Value--;
            obstacle.Text.SetText(obstacle.Value.ToString());
            BallDespawn(ball);
            if (obstacle.Value == 0)
                EntityDespawn(obstacle);
            if(_balls.Count == 0)
                LoseGame();
        }
        
        private void SpawnFinish()
        {
            _levelCompleted = false;
            if (_finish && _entitiesPool.Contains(_finish))
            {
                _finish.gameObject.SetActive(true);
                _entitiesPool.Remove(_finish);
            }
            else
            {
                _finish = Instantiate(_finishPrefab, _entitiesContainer);
            }
            _finish.Rect.anchoredPosition = _finishSpawnPosition;
            _entities.Add(_finish);
        }

        protected void SpawnWave()
        {
            if (_levelCompleted)
            {
                SpawnFinish();
                return;
            }
            var wave = _levelData.Waves[_wave];
            for (int i = 0; i < wave.Numbers.Length; i++)
            {
                var number = wave.Numbers[i];
                if(number == 0)
                    continue;
                var entity = SpawnEntity(wave.Type);
                entity.Rect.anchoredPosition = _obstacleStartPosition + _obstacleStep * i;
                entity.Value = number;
                if(entity.Text)
                    entity.Text.SetText(number.ToString());
            }
            _wave++;
            if (_wave == _levelData.Waves.Count)
            {
                if (_mode == 1)
                {
                    _wave = 0;
                    _levelData = LevelGenerator.Generate(_levelData);
                    _timeLeft = _levelData.Waves[_wave].Delay;
                    if (_timeLeft == 0)
                        SpawnWave();
                    return;
                }
                _timeLeft = 1f;
                _levelCompleted = true;
                return;   
            }
            _timeLeft = _levelData.Waves[_wave].Delay;
            if (_timeLeft == 0)
                SpawnWave();
        }
        
        private void WinGame()
        {
            DespawnAll();
            _gameStarted = false;
            NeedUpdate = false;
            GameEvents.InvokeWin();
        }
        
        private void LoseGame()
        {
            DespawnAll();
            _gameStarted = false;
            NeedUpdate = false;
            GameEvents.InvokeLose();
        }

        private void OnDestroy()
        {
            foreach (var ball in _ballsPool)
                Destroy(ball);
        }
    }
}