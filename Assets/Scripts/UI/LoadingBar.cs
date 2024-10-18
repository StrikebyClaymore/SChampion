using Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class LoadingBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private float _minValue;
        public bool NeedUpdate { get; private set; } = false;
        public bool InstantLoad;
        public readonly UnityEvent OnLoaded = new();

        public void StartLoading()
        {
            if (InstantLoad)
            {
                NeedUpdate = false;
                OnLoaded.Invoke();
                return;
            }
            _slider.value = _minValue;
            gameObject.Show();
            NeedUpdate = true;
        }

        public void CustomUpdate()
        {
            _slider.value += Time.deltaTime;
            if (_slider.value == _slider.maxValue)
            {
                NeedUpdate = false;
                OnLoaded.Invoke();
            }
        }
        
        private void OnDisable()
        {
            NeedUpdate = false;
        }
    }
}
