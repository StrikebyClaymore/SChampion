using System;
using DG.Tweening;
using Extensions;
using Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameTypePanel : MonoBehaviour
    {
        [field: SerializeField] public RectTransform Rect { get; private set; }
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _unlockText;
        [SerializeField] private Vector3 _notSelectedScale = new Vector3(0.76f, 0.76f, 1);
        private Sprite _unlockSprite;
        private Sprite _blurSprite;
        private bool _isSelected;
        private bool _isUnlocked;

        public void Initialize(GameTypeData data, bool unlock, string lockText = "")
        {
            _nameText.text = data.Name;
            _unlockSprite = data.Sprite;
            _blurSprite = data.LockSprite;
            _unlockText.gameObject.Hide();
            transform.localScale = _notSelectedScale;
            if (unlock)
            {
                Unlock();
            }
            else
            {
                _icon.sprite = data.LockSprite;
                _unlockText.text = lockText;
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public void Unlock()
        {
            _isUnlocked = true;
            if (_isSelected)
                _icon.sprite = _unlockSprite;
            else
                _icon.sprite = _blurSprite;
        }
        
        public void Select()
        {
            transform.DOKill();
            _isSelected = true;
            if (_isUnlocked)
            {
                _icon.sprite = _unlockSprite;
            }
            else
            {
                _icon.sprite = _blurSprite;
                _unlockText.gameObject.Show();
            }
            transform.DOScale(Vector3.one, 0.3f);
        }
        
        public void Deselect()
        {
            transform.DOKill();
            _isSelected = false;
            _icon.sprite = _blurSprite;
            transform.DOScale(_notSelectedScale, 0.3f);
            if(_isUnlocked)
                return;
            _unlockText.gameObject.Hide();
        }
    }
}