using Extensions;
using Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameTypePanel : PanelBase
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _unlockText;
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
        }
        
        public void Deselect()
        {
            _isSelected = false;
            _icon.sprite = _blurSprite;
            if(_isUnlocked)
                return;
            _unlockText.gameObject.Hide();
        }
    }
}