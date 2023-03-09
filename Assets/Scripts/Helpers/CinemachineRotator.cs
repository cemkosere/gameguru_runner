using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace Helpers
{
    public class CinemachineRotator : MonoBehaviour
    {
        private CinemachineVirtualCamera _cinemachine;
        private Tween _rotatingTween;
        private void OnEnable()
        {
            _cinemachine = GetComponent<CinemachineVirtualCamera>();
            StartRotating();
        }

        private void OnDisable()
        {
            StopRotating();
        }

       

        private void StartRotating()
        {
            var orbitalTransposer = _cinemachine.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            orbitalTransposer.m_Heading.m_Bias = 180;
            var bias = 180;
            _rotatingTween = DOTween
                .To(() => bias, x => bias = x, -180, 5)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart)
                .OnUpdate(() => orbitalTransposer.m_Heading.m_Bias = bias);
        }

        private void StopRotating()
        {
            _rotatingTween.Kill();
        }
    }
}