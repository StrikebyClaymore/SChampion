using TMPro;
using UnityEngine;

namespace Systems.Game.Entities
{
    public class Entity : MonoBehaviour
    {
        [field: SerializeField] public RectTransform Rect { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Text { get; private set; }
        [field: SerializeField] public EEntities Type { get; private set; }
        public int Value;

        private void OnValidate()
        {
            Rect = GetComponent<RectTransform>();
            Text = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Release()
        {
            gameObject.SetActive(false);
        }
    }
}