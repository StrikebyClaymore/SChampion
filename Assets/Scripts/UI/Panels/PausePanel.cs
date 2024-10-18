using TMPro;
using UI.Components;
using UnityEngine;

namespace UI
{
    public class PausePanel : PanelBase
    {
        [field: SerializeField] public TextMeshProUGUI BalanceText { get; private set; }
        [field: SerializeField] public CustomPushButton MenuButton { get; private set; }
        [field: SerializeField] public CustomPushButton BackButton { get; private set; }

        public void SetColors(int gameType)
        {
            MenuButton.SetButtonTopColors((EColorPalette)gameType);
            BackButton.SetButtonTopColors((EColorPalette)gameType);
        }
    }
}