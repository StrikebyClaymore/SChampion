using Extensions;
using Systems.Game;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems
{
    public class GameManager : MonoBehaviour
    {
        #region Fields and Properties

        [Header("Loading")]
        [SerializeField] private LoadingBar _loadingBar;
        [Header("Main")]
        [SerializeField] private GameObject _main;
        [SerializeField] private MenuPanel _menu;
        [SerializeField] private SettingsPanel _settings;
        [SerializeField] private RecordsPanel _records;
        [SerializeField] private LevelsPanel _levels;
        [FormerlySerializedAs("_gamePlay")]
        [Header("Gameplay")]
        [SerializeField] private GameObject _gameplay;
        [SerializeField] private StartTimerPanel _startTimer;
        [SerializeField] private PausePanel _pause;
        [SerializeField] private GameOverPanel _gameOver;
        [Header("Tutorial")]
        [SerializeField] private Tutorial _tutorial;
        [Header("Prefabs")]
        [SerializeField] private GameBase _footballPrefab;
        [SerializeField] private GameBase _basketballPrefab;
        [Header("Another")]
        [SerializeField] private AudioSource _backgroundAudioSource;
        [SerializeField] private AudioSource _uiAudioSource;
        [SerializeField] private AudioSource _effectsSource;
        [SerializeField] private AudioClip _levelCompletedClip;
        private GameBase _gameInstance;
        private int _gameType;
        private int _levelMode;
        private bool _isInitialized;

        #endregion
        
        #region Initialization

        private void Awake()
        {
            DisableAll();
            Initialize();
            _loadingBar.StartLoading();
        }

        private void Initialize()
        {
            if (_isInitialized)
                return;
            _loadingBar.OnLoaded.AddListener(LoadingCompleted);
            _tutorial.OnCompleted.AddListener(TutorialCompleted);
            _menu.Initialize();
            _menu.SettingsButton.OnReleased.AddListener(SettingsPressed);
            _menu.PlayButton.OnReleased.AddListener(PlayPressed);
            _menu.LevelsButton.OnPressed.AddListener(SwitchLevelMode);
            _menu.OnSwitchGameType.AddListener(SwitchGameType);
            _menu.GameButton.OnPressed.AddListener(SwitchGameMode);
            _settings.BackButton.OnReleased.AddListener(SettingsBackPressed);
            _settings.MusicRightButton.OnReleased.AddListener(AddMusic);
            _settings.MusicLeftButton.OnReleased.AddListener(RemoveMusic);
            _settings.SoundRightButton.OnReleased.AddListener(AddSound);
            _settings.SoundLeftButton.OnReleased.AddListener(RemoveSound);
            _records.GameButton.OnPressed.AddListener(SwitchRecordsGameMode);
            _levels.BackButton.OnReleased.AddListener(LevelsBackPressed);
            _levels.PlayButton.OnReleased.AddListener(LevelsPlayPressed);
            _startTimer.Timer.OnTimeTick.AddListener(StartTimerTick);
            _startTimer.Timer.OnCompleted.AddListener(StartTimerCompleted);
            _pause.BackButton.OnReleased.AddListener(PauseBackPressed);
            _pause.MenuButton.OnReleased.AddListener(PauseMenuPressed);
            _gameOver.RestartButton.OnReleased.AddListener(GameOverRestartPressed);
            _gameOver.MenuButton.OnReleased.AddListener(GameOverMenuPressed);
            GameEvents.OnMoneyChanged.AddListener(MoneyChanged);
            GameEvents.OnWin.AddListener(WinGame);
            GameEvents.OnLose.AddListener(LoseGame);
            GameEvents.OnUIPressed.AddListener(UIPressed);
            SetDefaultData();
            SetDefaultSettings();
            _isInitialized = true;
        }

        private void SetDefaultData()
        {
            var gameType = EGameTypes.Football;
            var bestScore = PlayerData.GetBestScore(gameType, ERecordPlace.First);
            PlayerData.SetBestScore(gameType, ERecordPlace.First, bestScore);
            bestScore = PlayerData.GetBestScore(gameType, ERecordPlace.Second);
            PlayerData.SetBestScore(gameType, ERecordPlace.Second, bestScore);
            bestScore = PlayerData.GetBestScore(gameType, ERecordPlace.Third);
            PlayerData.SetBestScore(gameType, ERecordPlace.Third, bestScore);
            
            gameType = EGameTypes.Basketball;
            bestScore = PlayerData.GetBestScore(gameType, ERecordPlace.First);
            PlayerData.SetBestScore(gameType, ERecordPlace.First, bestScore);
            bestScore = PlayerData.GetBestScore(gameType, ERecordPlace.Second);
            PlayerData.SetBestScore(gameType, ERecordPlace.Second, bestScore);
            bestScore = PlayerData.GetBestScore(gameType, ERecordPlace.Third);
            PlayerData.SetBestScore(gameType, ERecordPlace.Third, bestScore);
            
            var money = PlayerData.Data.GetInt(GameConstants.MONEY_SAVE_KEY, 0);
            var levelCompleted = PlayerData.GetLevelCompleted(EGameTypes.Football);
            PlayerData.SetLevelCompleted(EGameTypes.Football, levelCompleted);
            levelCompleted = PlayerData.GetLevelCompleted(EGameTypes.Basketball);
            PlayerData.SetLevelCompleted(EGameTypes.Basketball, levelCompleted);
            levelCompleted = PlayerData.GetLevelCompleted(EGameTypes.Tennis);
            PlayerData.SetLevelCompleted(EGameTypes.Tennis, levelCompleted);
            
            PlayerData.Data.SetInt(GameConstants.MONEY_SAVE_KEY, money);
            GameEvents.InvokeMoneyChanged(money);
        }
        
        private void SetDefaultSettings()
        {
            var music = PlayerData.Data.GetInt(GameConstants.MUSIC_SAVE_KEY, 100);
            var sound = PlayerData.Data.GetInt(GameConstants.SOUND_SAVE_KEY, 100);
            PlayerData.Data.SetInt(GameConstants.MUSIC_SAVE_KEY, music);
            PlayerData.Data.SetInt(GameConstants.SOUND_SAVE_KEY, sound);
            _settings.MusicText.text = $"{music} %";
            _settings.SoundText.text = $"{sound} %";
            _backgroundAudioSource.volume = (float)music / 100;
            AudioListener.volume = (float)sound / 100;
        }

        #endregion
        
        #region Base
        
        private void Update()
        {
            if(_loadingBar.NeedUpdate)
                _loadingBar.CustomUpdate();
            if(_gameInstance && _gameInstance.NeedUpdate)
                _gameInstance.CustomUpdate();
        }

        private void DisableAll()
        {
            _loadingBar.gameObject.Hide();
            _main.Hide();
            _menu.Hide();
            _settings.Hide();
            _records.Hide();
            _levels.Hide();
            _gameplay.Hide();
            _startTimer.Hide();
            _pause.Hide();
            _gameOver.Hide();
            _tutorial.gameObject.Hide();
        }

        private void MoneyChanged(int value)
        {
            var strValue = value.ToString();
            _menu.MoneyChanged(value);
            if(_gameInstance)
                _gameInstance.BalanceText.SetText(strValue);
            _pause.BalanceText.SetText(strValue);
            _gameOver.BalanceText.SetText(strValue);
        }
        
        private void UIPressed()
        {
            _uiAudioSource.Play();
        }
        
        #endregion

        #region Loading

        private void LoadingCompleted()
        {
            _loadingBar.gameObject.Hide();
            if (!PlayerData.GetTutorialCompleted())
            {
                _tutorial.StartTutorial();
            }
            else
            {
                _main.Show();
                _menu.Show();
            }
        }

        #endregion

        #region Tutorial

        private void TutorialCompleted()
        {
            PlayerData.SetTutorialCompleted();
            _tutorial.gameObject.Hide();
            _main.Show();
            _menu.Show();
        }

        #endregion
        
        #region Main
        
        #region Menu

        private void SettingsPressed()
        {
            _menu.Hide();
            _settings.Show();
        }
        
        private void PlayPressed()
        {
            if (!PlayerData.GetGameTypeUnlocked((EGameTypes)_gameType))
            {
                return;
            }
            _menu.Hide();
            if (_levelMode == 1)
                LevelsPlayPressed();
            else
                _levels.Open(_gameType);
        }

        private void SwitchLevelMode()
        {
            _levelMode = _levelMode == 0 ? 1 : 0;
        }

        private void SwitchGameType(int type)
        {
            _gameType = type;
        }
        
        private void SwitchGameMode()
        {
            _menu.Hide();
            _records.Show();
        }

        #endregion

        #region Settings

        private void SettingsBackPressed()
        {
            _settings.Hide();
            _menu.Show();
        }

        private void AddMusic()
        {
            var volume = ChangeSettingsValue(GameConstants.MUSIC_SAVE_KEY, 1);
            _backgroundAudioSource.volume = (float)volume / 100;
            _settings.MusicText.text = $"{volume} %";
        }
        
        private void RemoveMusic()
        {
            var volume = ChangeSettingsValue(GameConstants.MUSIC_SAVE_KEY, -1);
            _backgroundAudioSource.volume = (float)volume / 100;
            _settings.MusicText.text = $"{volume} %";
        }
        
        private void AddSound()
        {
            var volume = ChangeSettingsValue(GameConstants.SOUND_SAVE_KEY, 1);
            AudioListener.volume = (float)volume / 100;
            _settings.SoundText.text = $"{volume} %";
        }
        
        private void RemoveSound()
        {
            var volume = ChangeSettingsValue(GameConstants.SOUND_SAVE_KEY, -1);
            AudioListener.volume = (float)volume / 100;
            _settings.SoundText.text = $"{volume} %";
        }

        private int ChangeSettingsValue(string name, int direction)
        {
            var value = PlayerData.GetInt(name);
            if((direction < 0 && value == 0) || (direction > 0 && value == 100))
                return value;
            var nextValue = Mathf.Clamp(value + GameConstants.SETTINGS_STEP * direction, 0, 100);
            if (direction > 0)
                PlayerData.SetIntValue(name, nextValue);
            else
                PlayerData.SetIntValue(name, nextValue);
            return nextValue;
        }

        #endregion

        #region Records

        private void SwitchRecordsGameMode()
        {
            _menu.Show();
            _records.Hide();
        }

        #endregion

        #region Levels

        private void LevelsBackPressed()
        {
            _levels.Hide();
            _menu.Show();
        }

        private void LevelsPlayPressed()
        {
            _levels.Hide();
            _main.Hide();
            _gameplay.Show();
            _pause.SetColors(_gameType);
            _gameOver.SetColors(_gameType);
            if (!_gameInstance || (int)_gameInstance.Type != _gameType)
            {
                switch ((EGameTypes)_gameType)
                {
                    case EGameTypes.Football:
                        _gameInstance = Instantiate(_footballPrefab, _gameplay.transform);
                        break;
                    case EGameTypes.Basketball:
                        _gameInstance = Instantiate(_basketballPrefab, _gameplay.transform);
                        break;
                }
                _gameInstance.PauseButton.OnReleased.AddListener(PausePressed);
            }
            InitializeGame();
        }
        
        #endregion

        #endregion

        #region Gameplay

        #region Game

        private void InitializeGame()
        {
            _gameInstance.Show();
            _gameInstance.transform.SetAsFirstSibling();
            _gameInstance.BalanceText.SetText(PlayerData.GetInt(GameConstants.MONEY_SAVE_KEY).ToString());
            _startTimer.Show();
            _gameInstance.Reset();
            _startTimer.Timer.Enable();
        }

        private void StartTimerTick(float timeLeft)
        {
            int intTimeLeft = Mathf.CeilToInt(timeLeft);
            _startTimer.TimerText.SetText(intTimeLeft.ToString());
        }

        private void StartTimerCompleted()
        {
            _startTimer.Hide();
            _backgroundAudioSource.Play();
            if (_levelMode == 0)
            {
                _gameInstance.StartGame(_levels.GetLevelData());
            }
            else
            {
                _gameInstance.StartInfinityGame();
            }
        }

        private void PausePressed()
        {
            _backgroundAudioSource.Pause();
            _gameInstance.Hide();
            _pause.Show();
        }

        #endregion

        #region Pause

        private void PauseBackPressed()
        {
            _backgroundAudioSource.UnPause();
            _pause.Hide();
            _gameInstance.Show();
        }
        
        private void PauseMenuPressed()
        {
            Destroy(_gameInstance);
            _backgroundAudioSource.Stop();
            _pause.Hide();
            _gameplay.Hide();
            _main.Show();
            _menu.Show();
        }
    
        #endregion

        #region Game Over

        private void WinGame()
        {
            _backgroundAudioSource.Stop();
            _gameInstance.Hide();
            _gameplay.Hide();
            _main.Show();
            _levels.Show();
            _levels.LevelComplete();
            _effectsSource.clip = _levelCompletedClip;
            _effectsSource.Play();
        }

        private void LoseGame()
        {
            _backgroundAudioSource.Stop();
            _gameInstance.Hide();
            if (_levelMode == 0)
            {
                _gameplay.Hide();
                _main.Show();
                _levels.Show();
            }
            else
            {
                var score = Mathf.FloorToInt(_gameInstance.Timer);
                var newRecord = SaveNewScore(score);
                _gameOver.Show(newRecord, score);
            }
        }

        private bool SaveNewScore(int score)
        {
            var gameType = (EGameTypes)_gameType;
            var first = PlayerData.GetBestScore(gameType, ERecordPlace.First);
            var second = PlayerData.GetBestScore(gameType, ERecordPlace.Second);
            if (score > first)
            {
                PlayerData.SetBestScore(gameType, ERecordPlace.First, score);
                PlayerData.SetBestScore(gameType, ERecordPlace.Second, first);
                PlayerData.SetBestScore(gameType, ERecordPlace.Third, second);
                return true;
            }
            else if (score == first || score > second)
            {
                PlayerData.SetBestScore(gameType, ERecordPlace.Second, score);
                PlayerData.SetBestScore(gameType, ERecordPlace.Third, second);
            }
            else
            {
                PlayerData.SetBestScore(gameType, ERecordPlace.Third, score);
            }
            return false;
        }
        
        private void GameOverRestartPressed()
        {
            _gameOver.Hide();
            _gameplay.Hide();
            LevelsPlayPressed();
        }
        
        private void GameOverMenuPressed()
        {
            Destroy(_gameInstance);
            _backgroundAudioSource.Stop();
            _gameOver.Hide();
            _gameplay.Hide();
            _main.Show();
            _menu.Show();
        }

        #endregion
        
        #endregion
    }
}