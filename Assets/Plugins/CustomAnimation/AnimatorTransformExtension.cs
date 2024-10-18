using UnityEngine;

namespace CustomAnimation
{
    public static class AnimatorTransformExtension
    {
        public static void SetDataFromKey(this Transform transform, TransformAnimationKey key)
        {
            transform.localPosition = key.LocalPosition;
            transform.localEulerAngles = key.LocalEulerAngles;
            transform.localScale = key.LocalScale;
        }

        public static TransformAnimationKey GetDataForKey(this Transform transform) => new TransformAnimationKey(transform);
    }
}