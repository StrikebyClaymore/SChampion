using Systems;
using TMPro;
using UI.Components;
using UnityEngine;

namespace UI
{
    public class RecordsPanel : PanelBase
    {
        [field: SerializeField] public StateSwitchButton TypeButton { get; private set; }
        [field: SerializeField] public StateSwitchButton GameButton { get; private set; }
        [SerializeField] private TextMeshProUGUI _firstText;
        [SerializeField] private TextMeshProUGUI _secondText;
        [SerializeField] private TextMeshProUGUI _thirdText;
        private const string EMPTY_FORMAT = "-- / --";
        private int _recordType;

        private void Awake()
        {
            TypeButton.OnPressed.AddListener(SwitchRecordsType);
        }

        public override void Show()
        {
            var gameType = (EGameTypes)_recordType;
            var seconds = PlayerData.GetBestScore(gameType, ERecordPlace.First);
            var timeFormatted = $"{Mathf.FloorToInt((float)seconds/60)} : {seconds:00}";
            _firstText.SetText(seconds > 0 ? timeFormatted : EMPTY_FORMAT);
            seconds = PlayerData.GetBestScore(gameType, ERecordPlace.Second);
            timeFormatted = $"{Mathf.FloorToInt((float)seconds/60)} : {seconds:00}";
            _secondText.SetText(seconds > 0 ? timeFormatted : EMPTY_FORMAT);
            seconds = PlayerData.GetBestScore(gameType, ERecordPlace.Third);
            timeFormatted = $"{Mathf.FloorToInt((float)seconds/60)} : {seconds:00}";
            _thirdText.SetText(seconds > 0 ? timeFormatted : EMPTY_FORMAT);
            base.Show();
        }

        public override void Hide()
        {
            _recordType = 0;
            TypeButton.Reset();
            base.Hide();
        }

        private void SwitchRecordsType()
        {
            _recordType++;
            if (_recordType >= 3)
                _recordType = 0;
            Show();
        }
    }
}