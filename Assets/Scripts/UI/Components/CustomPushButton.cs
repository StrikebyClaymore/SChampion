using Extensions;
using Systems;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Components
{
    public class CustomPushButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public readonly UnityEvent OnPressed = new();
        public readonly UnityEvent OnReleased = new();
        
        [SerializeField] private Image _topBackground;
        [SerializeField] private Image _topFrame;
        [SerializeField] private Image _topBackground2;
        [SerializeField] private Image _topFrame2;

        public void OnPointerDown(PointerEventData eventData)
        {
            _topBackground2.gameObject.Hide();
            _topFrame2.gameObject.Hide();
            OnPressed?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _topBackground2.gameObject.Show();
            _topFrame2.gameObject.Show();
            GameEvents.InvokeUIPressed();
            OnReleased?.Invoke();
        }

        public void SetButtonTopColors(EColorPalette palette)
        {
            Color background = Color.white;
            Color background2 = Color.white;
            Color frame = Color.white;
            switch (palette)
            {
                case EColorPalette.Green:
                    ColorUtility.TryParseHtmlString(GameConstants.BOTTOM_GREEN_COLOR, out background);
                    ColorUtility.TryParseHtmlString(GameConstants.TOP_GREEN_COLOR, out background2);
                    ColorUtility.TryParseHtmlString(GameConstants.FRAME_GREEN_COLOR, out frame);
                    break;
                case EColorPalette.Yellow:
                    ColorUtility.TryParseHtmlString(GameConstants.BOTTOM_YELLOW_COLOR, out background);
                    ColorUtility.TryParseHtmlString(GameConstants.TOP_YELLOW_COLOR, out background2);
                    ColorUtility.TryParseHtmlString(GameConstants.FRAME_YELLOW_COLOR, out frame);
                    break;
            }
            SetButtonTopColors(background, background2, frame);
        }
        
        public void SetButtonTopColors(Color background, Color background2, Color frame)
        {
            _topBackground.color = background;
            _topBackground2.color = background2;
            _topFrame.color = frame;
            _topFrame2.color = frame;
        }
        
        private void OnValidate()
        {
            var parent = transform.Find("ButtonTop");
            if (parent == null)
                parent = transform;
            _topBackground = parent.Find("Background").GetComponent<Image>();
            _topFrame = parent.Find("Frame").GetComponent<Image>();
            _topBackground2 = parent.Find("Background2").GetComponent<Image>();
            _topFrame2 = parent.Find("Frame2").GetComponent<Image>();
        }
    }
}