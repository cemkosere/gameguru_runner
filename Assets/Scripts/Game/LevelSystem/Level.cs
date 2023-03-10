using System.Collections.Generic;
using DG.Tweening;
using Game.MeshSystem;
using Game.PlayerSystem;
using Game.Pools;
using Management;
using UnityEngine;

namespace Game.LevelSystem
{
    public class Level : MonoBehaviour, ILevel
    {
        public static bool IsCompleted;
        
        public Transform playerSpawnPoint;
        public Transform finishLine;
        public Transform endPoint;
        public int platformCount = 10;
        
        private IPlayer _player;
        private List<MainMesh> _allMeshes;
        private Queue<MainMesh> _mainMeshes;
        private Queue<MainMesh> _road;
        private MainMesh _lastMesh;
        private MainMesh _currentMesh;
        private Tween _meshMovementTween;

        
        

        public void Initialize(IPlayer player)
        {
            _player = player;
            InitializeRoad();
            
        }

        public void InitializeRoad()
        {
            IsCompleted = false;
            _allMeshes = new List<MainMesh>();
            _mainMeshes = new Queue<MainMesh>();
            _road = new Queue<MainMesh>();
            finishLine.position = transform.position + Vector3.forward * (platformCount * GameConstants.MainMeshLength + 1);
            
            for (var i = 0; i < platformCount; i++)
            {
                var rentMesh = MainMeshPool.Instance.Rent();
                _allMeshes.Add(rentMesh);
                _mainMeshes.Enqueue(rentMesh);
            }

            var mesh = _mainMeshes.Dequeue();
            mesh.SetScaleAndPosition(new Vector3(5,1,GameConstants.MainMeshLength), playerSpawnPoint.position);
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

        public void StartLoop()
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
                _meshMovementTween = _currentMesh.StartMovement();
                GameActions.OnTap += OnTap;
            }
        }

        public void OnTap()
        {
            GameActions.OnTap -= OnTap;
            var status = StopMeshMovement(_currentMesh);
            if (!status) return;
            
            _road.Enqueue(_currentMesh);
            _lastMesh = _currentMesh;
            _player.AddNewPoint(_currentMesh.Position);
            _player.AddNewPoint(_currentMesh.endPoint);
            StartLoop();

        }
        
        private bool StopMeshMovement(MainMesh mainMesh)
        {
            _meshMovementTween.Pause();
            return mainMesh.CalculateScale(_lastMesh);
        }
        
        public void Remove()
        {
            foreach (var mesh in _allMeshes)
            {
                MainMeshPool.Instance.Return(mesh);
            }
            _meshMovementTween.Kill();
            GameActions.OnTap -= OnTap;
            Destroy(gameObject);
        }

    }
}