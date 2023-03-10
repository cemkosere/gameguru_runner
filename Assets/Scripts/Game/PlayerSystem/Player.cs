using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.LevelSystem;
using UnityEngine;

namespace Game.PlayerSystem
{
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] private Animator animator;
        
        private Queue<Vector3> _points = new ();
        private bool _isMoving = false;
        private bool _isDancing = false;
        private Tween _movingTween;
        private Sequence _fallingSequence;
        public void AddNewPoint(Vector3 point)
        {
            _points.Enqueue(point);
            if(_isMoving) return;
            Move();
        }
        public void Move()
        {
            if (_points.Count <= 0)
            {
                if (Level.IsCompleted)
                {
                    GameActions.LevelEnd?.Invoke(true);
                    Dance();
                }
                else
                {
                    _movingTween.Kill();
                    _fallingSequence = DOTween.Sequence();
                    _fallingSequence
                        .Append(transform.DOMoveZ(transform.position.z + 1, .1f))
                        .Append(transform.DOMoveY(transform.position.y - 10, 1f))
                        .OnComplete(() =>
                        {
                            
                            GameActions.LevelEnd?.Invoke(false);

                        });
                }

                _isMoving = false;
                return;
            }
            _isMoving = true;
            var point = _points.Dequeue();
            MovePoint(point, Move);
        }
        public void MovePoint(Vector3 point, Action callBack = null)
        {
            var callBackInvoked = false;
            Walk();
            _movingTween = transform
                .DOLookAt(point, 1000f)
                .SetSpeedBased(true)
                .OnComplete(() =>
                    transform
                        .DOMove(point, GameConstants.PlayerSpeed)
                        .SetSpeedBased(true)
                        .SetEase(Ease.Linear)
                        .OnUpdate(() =>
                        {
                            //su kadar mesafe kaldiginda look at basla
                            if (Vector3.Distance(transform.position, point) < 1)
                            {
                                if(callBackInvoked)return;
                                callBack?.Invoke();
                                callBackInvoked = true;
                            }
                        }));
        }
        public void Walk()
        {
            if(!_isDancing) return;
            animator.SetTrigger("Walk");
            _isDancing = false;
        }
        public void Dance()
        {
            if (_isDancing) return;
            animator.SetTrigger("Dance");
            _isDancing = true;
        }

        private void OnDestroy()
        {
            _movingTween.Kill();
            _fallingSequence.Kill();
        }
    }
}