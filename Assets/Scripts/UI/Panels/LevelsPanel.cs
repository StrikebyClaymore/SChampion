using Extensions;
using Systems;
using TMPro;
using UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelsPanel : PanelBase
    {
        [field: SerializeField] public CustomPushButton BackButton { get; private set; }
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private GameObject _levelCompletePanel;
        [SerializeField] private TextMeshProUGUI _levelCompleteText;
        private const string LEVEL_COMPLETE_TEXT_FORMAT = "LEVEL {0} COMPLETE";
        [field: SerializeField] public CustomPushButton PlayButton { get; private set; }
        [SerializeField] private LevelTypeData[] _levelTypes;
        [SerializeField] private Transform[] _cells;
        [SerializeField] private GameObject _selectedCube;
        private int _type;
        private int _level;

        private void Awake()
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                var cell = _cells[i];
                var button = cell.GetChild(0).GetComponent<Button>();
                var index = i;
                button.onClick.AddListener(() => CellClick(index));
            }
            _levelCompletePanel.Hide();
        }

        public void Open(int type)
        {
            _type = type;
            BackButton.SetButtonTopColors((EColorPalette)_type);
            PlayButton.SetButtonTopColors((EColorPalette)_type);
            Show();
            Redraw();
        }

        public override void Hide()
        {
            base.Hide();
            _levelCompletePanel.Hide();
        }

        public LevelData GetLevelData()
        {
            return _levelTypes[_type].Levels[_level];
        }
        
        public LevelTypeData GetGameData()
        {
            return _levelTypes[_type];
        }
        
        public void LevelComplete()
        {
            var type = (EGameTypes)_type;
            if(_level == PlayerData.GetLevelCompleted(type))
                PlayerData.SetLevelCompleted(type, _level + 1);
            Redraw();
            _levelCompleteText.text = string.Format(LEVEL_COMPLETE_TEXT_FORMAT, _level + 1);
            _levelCompletePanel.Show();
        }

        public void SetData(EGameTypes gameType, int level, LevelData data)
        {
            _levelTypes[(int)gameType].Levels[level] = data;
            Debug.Log($"Set {gameType} {level} successful");
        }
        
        private void Redraw()
        {
            var level = PlayerData.GetLevelCompleted((EGameTypes)_type) + 1;
            if (level > _levelTypes[_type].Levels.Length)
                level = _levelTypes[_type].Levels.Length;
            _selectedCube.GetComponent<Image>().sprite = _levelTypes[_type].SelectedSprite;
            foreach (var cell in _cells)
                cell.GetChild(1).gameObject.Hide();
            for (int i = 0; i < level; i++)
            {
                var cube = _cells[i].GetChild(1).GetComponent<Image>();
                cube.sprite = _levelTypes[_type].BaseSprite;
                cube.gameObject.Show();
            }
            CellClick(level);
        }

        private void CellClick(int index)
        {
            if(_levelCompletePanel.activeSelf)
                _levelCompletePanel.Hide();
            var level = PlayerData.GetLevelCompleted((EGameTypes)_type) + 1;
            if (level > _levelTypes[_type].Levels.Length)
                level = _levelTypes[_type].Levels.Length;
            if (index >= level)
                index = level - 1;
            var parent = _cells[index].GetChild(1);
            _selectedCube.transform.SetParent(parent);
            _selectedCube.transform.localPosition = Vector3.zero;
            _selectedCube.Show();
            _levelText.text = (index + 1).ToString();
            _level = index;
        }
    }
}