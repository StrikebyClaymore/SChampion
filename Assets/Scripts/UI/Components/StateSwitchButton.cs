using System;
using Systems;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.Components
{
    public class StateSwitchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _buttonRect;
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private int _maxState = 1;
        [SerializeField] private string[] _statesText;
        private int _state = 0;
        [SerializeField] private int _initialState = 0;
        public readonly UnityEvent OnPressed = new();
        public readonly UnityEvent OnReleased = new();

        private void Awake()
        {
            SetState(_initialState);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            GameEvents.InvokeUIPressed();
            NextState();
            OnPressed?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnReleased?.Invoke();
        }

        public void Reset()
        {
            _state = _initialState;
            SetState(_state);
        }
        
        private void NextState()
        {
            _state++;
            if (_state > _maxState)
                _state = 0;
            SetState(_state);
        }

        private void SetState(int state)
        {
            _state = state;
            _buttonText.text = _statesText[_state];
            MoveButton();
        }

        private void MoveButton()
        {
            var step = _state * _buttonRect.rect.width;
            _buttonRect.offsetMin = new Vector2(step, 0);
            _buttonRect.offsetMax = new Vector2(step, 0);
        }

        private void OnDisable()
        {
            SetState(_initialState);
        }
    }
}