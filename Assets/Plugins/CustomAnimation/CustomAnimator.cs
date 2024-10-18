using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomAnimation
{
    public class CustomAnimator : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;
        [Space] public bool IsPlayOnEnable;
        public bool IsPlayOnStart;
        public bool IsLoop;
        /// <summary>
        /// After animation end animation play reverse if IsLoop enabled.
        /// </summary>
        public bool IsLoopReverse;
        private bool m_IsReversing;
        [Range(0f, float.MaxValue)] public float Delay = 0;
        public EasingFunction.Ease BaseEasing = EasingFunction.Ease.Linear;
        [field: SerializeField] public bool IsPlaying { get; private set; }
        [field: SerializeField] public float Progress { get; private set; }
        /// <summary>
        /// First key is start key. At Play its values copy to target Transform.
        /// </summary>
        [SerializeField, Space] private List<TransformAnimationKey> m_Keys = new();
        private Coroutine m_Coroutine;
        private bool m_IsInitialized;

        public void Start()
        {
            m_IsInitialized = true;
            if (IsPlayOnStart)
                Play();
        }

        private void OnEnable()
        {
            if (IsPlayOnEnable)
                Play();
        }

        private void OnDisable()
        {
            Stop();
        }

        [ContextMenu("Play")]
        public void Play()
        {
            if (IsPlaying || m_Target == null || m_Keys.Count == 0)
                return;
            TryStopCoroutine();
            m_Target.SetDataFromKey(m_Keys[0]);
            IsPlaying = true;
            m_Coroutine = StartCoroutine(Process());
        }

        [ContextMenu("Stop")]
        public void Stop()
        {
            if (!IsPlaying)
                return;
            TryStopCoroutine();
            m_Target.SetDataFromKey(m_Keys[0]);
            IsPlaying = false;
            m_IsReversing = false;
            Progress = 0f;
            foreach (var key in m_Keys)
                key.Progress = 0f;
        }

        private IEnumerator Process()
        {
            yield return new WaitForEndOfFrame();
            if (Delay > 0f)
                yield return new WaitForSecondsRealtime(Delay);
            var easingFunctionBase = EasingFunction.GetEasingFunction(BaseEasing);
            var curKeyIndex = m_IsReversing ? m_Keys.Count - 1 : 0;
            var nextKeyIndex = m_IsReversing ? curKeyIndex - 1 : 1;
            do
            {
                foreach (var key in m_Keys)
                    key.Progress = 0f;
                m_Keys[curKeyIndex].Progress = 1f;
                Progress = 1f / m_Keys.Count;

                while (Progress < 1.0f)
                {
                    var curKey = m_Keys[curKeyIndex];
                    var nextKey = m_Keys[nextKeyIndex];
                    var easingFunction = curKey.ISUseBaseEasing
                        ? easingFunctionBase
                        : EasingFunction.GetEasingFunction(curKey.Easing);

                    if (nextKey.Delay > 0f)
                        yield return new WaitForSecondsRealtime(nextKey.Delay);

                    while (nextKey.Progress < 1.0f)
                    {
                        var progressValue = Time.unscaledDeltaTime * (1.0f / nextKey.Duration);
                        nextKey.Progress += progressValue;
                        Progress += progressValue / m_Keys.Count;
                        m_Target.SetDataFromKey(TransformAnimationKey.Lerp(easingFunction, curKey,
                            nextKey, nextKey.Progress));
                        yield return null;
                    }

                    nextKey.Progress = 1f;

                    if ((m_IsReversing && nextKeyIndex == 0) || (!m_IsReversing && nextKeyIndex == m_Keys.Count - 1))
                    {
                        if (IsLoopReverse)
                            m_IsReversing = !m_IsReversing;
                        curKeyIndex = m_IsReversing ? m_Keys.Count - 1 : 0;
                        nextKeyIndex = m_IsReversing ? curKeyIndex - 1 : 1;
                        break;
                    }

                    curKeyIndex += m_IsReversing ? -1 : 1;
                    nextKeyIndex += m_IsReversing ? -1 : 1;
                }

            } while (IsLoop);

            IsPlaying = false;
        }

        private void TryStopCoroutine()
        {
            if (m_Coroutine == null) return;
            StopCoroutine(m_Coroutine);
            m_Coroutine = null;
        }

#if UNITY_EDITOR
        [ContextMenu("ResetToFirstKey")]
        private void ResetToFirstKey()
        {
            m_Target.SetDataFromKey(m_Keys[0]);
        }

        [ContextMenu("SetTransformToFirstKey")]
        private void SetTransformToFirstKey()
        {
            if (m_Keys.Count == 0)
                return;
            m_Keys[0].LocalPosition = m_Target.localPosition;
            m_Keys[0].LocalEulerAngles = m_Target.localEulerAngles;
            m_Keys[0].LocalScale = m_Target.localScale;
        }
#endif
    }
}