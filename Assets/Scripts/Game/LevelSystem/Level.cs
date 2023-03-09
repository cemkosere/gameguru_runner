using System.Collections.Generic;
using DG.Tweening;
using Game.MeshSystem;
using Game.PlayerSystem;
using Game.Pools;
using Management;
using UnityEngine;

namespace Game.LevelSystem
{
    public class Level : MonoBehaviour
    {
        public Transform playerSpawnPoint;
        public Transform finishLine;
        public Transform endPoint;
        public int platformCount = 10;
        
        private LevelManager _levelManager;
        private Player _player;
        private Queue<MainMesh> _mainMeshes;
        private Queue<MainMesh> _road;
        private MainMesh _lastMesh;
        private MainMesh _currentMesh;

        public static bool IsCompleted;
        

        public void Initialize(LevelManager levelManager, Player player)
        {
            _levelManager = levelManager;
            _player = player;
            InitializeRoad();
            
        }

        private void InitializeRoad()
        {
            IsCompleted = false;
            _mainMeshes = new Queue<MainMesh>();
            _road = new Queue<MainMesh>();
            finishLine.position = transform.position + Vector3.forward * (platformCount * 5 + 1);
            
            for (var i = 0; i < platformCount; i++)
            {
                _mainMeshes.Enqueue(MainMeshPool.Instance.Rent());
            }

            var mesh = _mainMeshes.Dequeue();
            mesh.SetScaleAndPosition(new Vector3(5,1,5), playerSpawnPoint.position);
            mesh.gameObject.SetActive(true);
            _lastMesh = mesh;
            _road.Enqueue(mesh);
        }

        public void _Start()
        {
            StartLoop();
            _player.AddNewPoint(_lastMesh.Position);
            _player.AddNewPoint(_lastMesh.endPoint);
        }

        private void StartLoop()
        {
            if (_mainMeshes.Count == 0)
            {
                IsCompleted = true;
                _player.AddNewPoint(finishLine.position);
            }
            else
            {
                _currentMesh = _mainMeshes.Dequeue();
                _currentMesh.SetScaleAndPosition(_lastMesh.Scale, new Vector3(6,0,_lastMesh.Position.z + _lastMesh.Scale.z));
                _currentMesh.gameObject.SetActive(true);
                StartMeshMovement(_currentMesh);
                GameActions.OnTap += OnTap;
            }
        }

        private void OnTap()
        {
            GameActions.OnTap -= OnTap;
            var status = StopMeshMovement(_currentMesh);
            if (status)
            {
                _road.Enqueue(_currentMesh);
                _lastMesh = _currentMesh;
                _player.AddNewPoint(_currentMesh.Position);
                _player.AddNewPoint(_currentMesh.endPoint);
                StartLoop();
            }
            else
            {
                //yeni dongu baslatma
                
            }
            
        }

        private Tween _meshMovementTween;
        private void StartMeshMovement(MainMesh mainMesh)
        {
            _meshMovementTween = mainMesh.Transform
                .DOMoveX(-6, GameConstants.RoadPieceSpeed)
                .SetSpeedBased(true)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }
        private bool StopMeshMovement(MainMesh mainMesh)
        {
            _meshMovementTween.Pause();
             return CalculateScale(mainMesh);
        }
        private bool CalculateScale(MainMesh mainMesh)
        {
            var diff = _lastMesh.Position.x - mainMesh.Position.x;
            var absDiff = Mathf.Abs(diff);
            if (absDiff < .25f)
            {//TOLERANCE
                mainMesh.Position = _lastMesh.Position + Vector3.forward * _lastMesh.Scale.z;
                //Perfect
                AudioManager.Instance.PerfectHit();
                return true;
            }
            AudioManager.Instance.ResetHit();
            var dropMesh = DropMeshPool.Instance.Rent();
            
            if (absDiff >= _lastMesh.Scale.x)
            {
                dropMesh.SetScaleAndPosition(_lastMesh.Scale, mainMesh.Position);
                MainMeshPool.Instance.Return(mainMesh);
                return false;
            }
            
            mainMesh.Scale -= Vector3.right * absDiff;
            mainMesh.Position += Vector3.right * (diff / 2);

            
            var x = ((mainMesh.Scale.x / 2) + (absDiff / 2) + Mathf.Abs(mainMesh.Position.x) ) * (diff < 0 ? 1 : -1);
            dropMesh.SetScaleAndPosition(
                new Vector3(absDiff,1,_lastMesh.Scale.z), 
                new Vector3(x,mainMesh.Position.y,mainMesh.Position.z));
            return true;
            
        }

    }
}