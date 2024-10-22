using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Systems.Game.Entities
{
    public class Entity : MonoBehaviour
    {
        [field: SerializeField] public RectTransform Rect { get; private set; }
        [field: SerializeField] public Collider2D Collider { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Text { get; private set; }
        [field: SerializeField] public EEntities Type { get; private set; }
        private Transform _parent;
        public int Value;

        private void Awake()
        {
            _parent = transform.parent;
        }

        public void Spawn()
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.one;
            Collider.enabled = true;
        }
        
        public void Release()
        {
            Collider.enabled = false;
            gameObject.SetActive(false);
            transform.SetParent(_parent);
        }

        [ContextMenu("AnimateRelease")]
        public void AnimateRelease()
        {
            Collider.enabled = false;
            var sequence = GameManager.Instance.AnimateEntityRelease(this);
            sequence.OnComplete(() =>
            {
                gameObject.SetActive(false);
                transform.SetParent(_parent);
                sequence.Kill();
            });
        }
        
        private void OnValidate()
        {
            Rect = GetComponent<RectTransform>();
            Collider = GetComponent<Collider2D>();
            Text = GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}