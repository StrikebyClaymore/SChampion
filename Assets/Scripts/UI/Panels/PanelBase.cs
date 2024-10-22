using DG.Tweening;
using UnityEngine;

namespace UI
{
    public abstract class PanelBase : MonoBehaviour
    {
        public bool IsAnimating { get; protected set; }

        public virtual void Show()
        {
            var canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.DOKill();
            gameObject.SetActive(true);
            IsAnimating = true;
            canvasGroup.DOFade(1, 0.3f).OnComplete(() =>
            {
                IsAnimating = false;
            });
        }
        
        public virtual void Hide()
        {
            var canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.DOKill();
            IsAnimating = true;
            canvasGroup.DOFade(0, 0.3f).OnComplete(() =>
            {
                IsAnimating = false;
                gameObject.SetActive(false);
            });
        }
    }
}