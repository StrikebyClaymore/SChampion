using Systems;
using TMPro;
using UnityEngine;

namespace UI
{
    public class StartTimerPanel : PanelBase
    {
        [field: SerializeField] public MonoTimer Timer { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TimerText { get; private set; }
    }
}