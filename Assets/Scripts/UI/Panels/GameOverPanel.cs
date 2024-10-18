using TMPro;
using UI.Components;
using UnityEngine;

namespace UI
{
    public class GameOverPanel : PanelBase
    {
        [field: SerializeField] public TextMeshProUGUI BalanceText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TimeText { get; private set; }
        [field: SerializeField] public GameObject RecordText { get; private set; }
        [field: SerializeField] public CustomPushButton RestartButton { get; private set; }
        [field: SerializeField] public CustomPushButton MenuButton { get; private set; }

        public void Show(bool newRecord, int score)
        {
            var timeFormatted = $"{Mathf.FloorToInt((float)score/60)} : {score:00}";
            TimeText.SetText(timeFormatted);
            RecordText.gameObject.SetActive(newRecord);
            base.Show();
        }

        public void SetColors(int gameType)
        {
            RestartButton.SetButtonTopColors((EColorPalette)gameType);
            MenuButton.SetButtonTopColors((EColorPalette)gameType);
        }
    }
}