using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtension
    {
        public static void Show(this GameObject gameObject)
        {
            gameObject.SetActive(true);
        }
        
        public static void Hide(this GameObject gameObject)
        {
            gameObject.SetActive(false);
        }
    }
}