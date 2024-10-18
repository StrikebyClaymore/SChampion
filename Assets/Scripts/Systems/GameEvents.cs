using UnityEngine.Events;

namespace Systems
{
    public static class GameEvents
    {
        public static readonly UnityEvent<int> OnMoneyChanged = new UnityEvent<int>();
        public static readonly UnityEvent OnWin = new UnityEvent();
        public static readonly UnityEvent OnLose = new UnityEvent();
        public static readonly UnityEvent OnUIPressed = new UnityEvent();

        public static void InvokeMoneyChanged(int money) => OnMoneyChanged.Invoke(money);
        public static void InvokeLose() => OnLose.Invoke();
        public static void InvokeWin() => OnWin.Invoke();

        public static void InvokeUIPressed() => OnUIPressed.Invoke();
    }
}