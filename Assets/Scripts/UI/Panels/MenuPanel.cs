using System.Collections.Generic;
using Extensions;
using Systems;
using TMPro;
using UI.Components;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class MenuPanel : PanelBase
    {
        [field: SerializeField] public TextMeshProUGUI BalanceText { get; private set; }
        [field: SerializeField] public CustomPushButton SettingsButton { get; private set; }
        [field: SerializeField] public CustomPushButton PlayButton { get; private set; }
        [field: SerializeField] public StateSwitchButton LevelsButton { get; private set; }
        [field: SerializeField] public StateSwitchButton GameButton { get; private set; }
        [SerializeField] private CustomPushButton _moveRightButton;
        [SerializeField] private CustomPushButton _moveLeftButton;
        [SerializeField] private Transform _typesContainer;
        [SerializeField] private GameTypePanel _gameTypePanelPrefab;
        [SerializeField] private GameTypeData[] _gameTypes;
        [SerializeField] private string LockTextFormat = "TO UNLOCK THIS MODE YOU NEED {0} STARS";
        [SerializeField] private Vector3 _notSelectedScale = new Vector3(0.76f, 0.76f, 1);
        [SerializeField] private Vector2 _notSelectedOffset = new Vector3(162, 0);
        private List<GameTypePanel> _panels = new List<GameTypePanel>();
        private int _currentPanelIndex;
        private bool _isInitialized = false;
        public readonly UnityEvent<int> OnSwitchGameType = new();
        
        public void Initialize()
        {
            if(_isInitialized)
                return;
            CreateTypes();
            _moveRightButton.OnReleased.AddListener(MoveRight);
            _moveLeftButton.OnReleased.AddListener(MoveLeft);
            _currentPanelIndex = 0;
            OnSwitchGameType?.Invoke(_currentPanelIndex);
            ShowLeftAndRight();
            _panels[0].Select();
            _isInitialized = true;
        }

        public void UnlockGame(EGameTypes type)
        {
            _gameTypes[(int)type].IsUnlocked = true;
            _panels[(int)type].Unlock();
            PlayerData.SetGameTypeUnlocked(type);
        }
        
        public void MoneyChanged(int value)
        {
            BalanceText.SetText(value.ToString());
            foreach (var data in _gameTypes)
            {
                if(!data.CanUnlock || data.IsUnlocked)
                    continue;
                if (value >= data.Cost)
                    UnlockGame(data.Type);
            }
        }
        
        private void CreateTypes()
        {
            foreach (var type in _gameTypes)
            {
                var instance = Instantiate(_gameTypePanelPrefab, _typesContainer);
                instance.Initialize(type, TypeIsUnlocked(type), GetLockText(type.Cost));
                _panels.Add(instance);
            }
            for (int i = 1; i < _panels.Count; i++)
            {
                var panel = _panels[i];
                panel.transform.localScale = _notSelectedScale;
                var step = _notSelectedOffset * i;
                panel.GetComponent<RectTransform>().anchoredPosition += step;
            }
        }

        private void MoveRight()
        {
            Move(1);
        }

        private void MoveLeft()
        {
            Move(-1);
        }

        private void Move(int direction)
        {
            var currPanel = _panels[_currentPanelIndex];
            currPanel.transform.localScale = _notSelectedScale;
            currPanel.Deselect();
            _currentPanelIndex += direction;
            currPanel = _panels[_currentPanelIndex];
            currPanel.transform.localScale = Vector3.one;
            currPanel.Select();
            for (int i = 0; i < _panels.Count; i++)
            {
                _panels[i].GetComponent<RectTransform>().anchoredPosition -= _notSelectedOffset * direction;
            }
            ShowLeftAndRight();
            OnSwitchGameType?.Invoke(_currentPanelIndex);
        }

        private void ShowLeftAndRight()
        {
            for (int i = 0; i < _panels.Count; i++)
            {
                var panel = _panels[i];
                if (i == _currentPanelIndex - 1 || i == _currentPanelIndex || i == _currentPanelIndex + 1)
                    panel.Show();
                else
                    panel.Hide();
            }
            _panels[_currentPanelIndex].transform.SetAsLastSibling();
            if (_currentPanelIndex == 0)
            {
                _moveLeftButton.gameObject.Hide();
            }
            else if (_currentPanelIndex == 1)
            {
                _moveLeftButton.gameObject.Show();
            }
            if (_currentPanelIndex == _panels.Count - 1)
            {
                _moveRightButton.gameObject.Hide();
            }
            else if (_currentPanelIndex == _panels.Count - 2)
            {
                _moveRightButton.gameObject.Show();
            }
        }
        
        private bool TypeIsUnlocked(GameTypeData type)
        {
            if (type.Cost == 0)
            {
                PlayerData.SetGameTypeUnlocked(type.Type);
                return true;
            }
            return PlayerData.GetGameTypeUnlocked(type.Type);
        }

        private string GetLockText(int cost)
        {
            return string.Format(LockTextFormat, cost);
        }
    }
}