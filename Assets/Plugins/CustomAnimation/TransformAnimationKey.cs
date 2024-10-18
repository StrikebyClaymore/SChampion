using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CustomAnimation
{
    [Serializable]
    public class TransformAnimationKey
    {
        public bool ISUseBaseEasing = true;
        public EasingFunction.Ease Easing = EasingFunction.Ease.Linear;
        [Range(0f, float.MaxValue)] public float Delay = 0;
        [Range(0f, float.MaxValue)] public float Duration = 0;
        public Vector3 LocalPosition;
        [FormerlySerializedAs("localEulerAngles")] public Vector3 LocalEulerAngles;
        public Vector3 LocalScale;
        public float Progress;

        public TransformAnimationKey()
        {
        }

        public TransformAnimationKey(Transform transform)
        {
            LocalEulerAngles = transform.localEulerAngles;
            LocalPosition = transform.localPosition;
            LocalScale = transform.localScale;
        }

        public static TransformAnimationKey Lerp(EasingFunction.Function easing, TransformAnimationKey from,
            TransformAnimationKey to, float t)
        {
            var result = new TransformAnimationKey();

            result.LocalPosition = new Vector3(
                easing.Invoke(from.LocalPosition.x, to.LocalPosition.x, t),
                easing.Invoke(from.LocalPosition.y, to.LocalPosition.y, t),
                easing.Invoke(from.LocalPosition.z, to.LocalPosition.z, t));

            result.LocalEulerAngles = new Vector3(
                easing.Invoke(from.LocalEulerAngles.x, to.LocalEulerAngles.x, t),
                easing.Invoke(from.LocalEulerAngles.y, to.LocalEulerAngles.y, t),
                easing.Invoke(from.LocalEulerAngles.z, to.LocalEulerAngles.z, t));
            
            result.LocalScale = new Vector3(
                easing.Invoke(from.LocalScale.x, to.LocalScale.x, t),
                easing.Invoke(from.LocalScale.y, to.LocalScale.y, t),
                easing.Invoke(from.LocalScale.z, to.LocalScale.z, t));

            return result;
        }
    }
}