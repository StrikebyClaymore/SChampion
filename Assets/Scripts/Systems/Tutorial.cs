using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Systems
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private Image _stageImage; 
        [SerializeField] private Button _nextButton; 
        [SerializeField] private Sprite[] _stageSprites; 
        private int _stage = -1;
        public readonly UnityEvent OnCompleted = new();
        
        public void StartTutorial()
        {
            _nextButton.onClick.AddListener(NextStage);
            gameObject.SetActive(true);
            NextStage();
        }

        private void NextStage()
        {
            _stage++;
            if (_stage == _stageSprites.Length)
            {
                OnCompleted?.Invoke();
                return;
            }
            _stageImage.sprite = _stageSprites[_stage];
        }
    }
}