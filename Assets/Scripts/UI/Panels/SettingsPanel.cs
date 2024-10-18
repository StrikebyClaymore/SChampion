using TMPro;
using UI.Components;
using UnityEngine;

namespace UI
{
    public class SettingsPanel : PanelBase
    {
        [field: SerializeField] public CustomPushButton BackButton { get; private set; }
        [field: SerializeField] public CustomPushButton MusicRightButton { get; private set; }
        [field: SerializeField] public CustomPushButton MusicLeftButton { get; private set; }
        [field: SerializeField] public CustomPushButton SoundRightButton { get; private set; }
        [field: SerializeField] public CustomPushButton SoundLeftButton { get; private set; }
        [field: SerializeField] public TextMeshProUGUI MusicText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI SoundText { get; private set; }
    }
}