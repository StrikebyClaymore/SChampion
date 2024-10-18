using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
    public class CustomSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private float _minVisualValue;
        [SerializeField] private float _maxVisualValue;

        public void SetValue(float value)
        {
            value = Mathf.Clamp(value, _minVisualValue, _maxVisualValue);
            _slider.value = value;
        }
    }
}