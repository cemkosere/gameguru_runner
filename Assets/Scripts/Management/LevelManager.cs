using Game;
using Game.LevelSystem;
using Game.PlayerSystem;
using UnityEngine;
using Zenject;

namespace Management
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Level levelPrefab;
        [SerializeField] private Player playerPrefab;
        public static Level CurrentLevel;
        public static Player CurrentPlayer;


        private CameraManager _cameraManager;
        [Inject]
        private void OnInject(CameraManager cameraManager)
        {
            _cameraManager = cameraManager;
        }
        
        public void InitializeNewLevel()
        {
            var spawnPosition = !CurrentLevel ? Vector3.zero : CurrentLevel.endPoint.position;
            SpawnLevel(spawnPosition);
            SpawnPlayer();
            CurrentLevel.Initialize(this, CurrentPlayer);
        }
        private void SpawnLevel(Vector3 position)
        {
            CurrentLevel = Instantiate(levelPrefab, position, Quaternion.identity);
        }

        private void SpawnPlayer()
        {
            if(!CurrentPlayer)
            {
                CurrentPlayer = 
                    Instantiate(playerPrefab, 
                        CurrentLevel.playerSpawnPoint.position, 
                        Quaternion.identity, 
                        CurrentLevel.transform);

                var playerTransform = CurrentPlayer.transform;
                _cameraManager.InitializeSingleCamera(CameraType.MainMenu, playerTransform, playerTransform);
                _cameraManager.InitializeSingleCamera(CameraType.GamePlay, playerTransform, playerTransform);
                _cameraManager.InitializeSingleCamera(CameraType.Success, playerTransform, playerTransform);
                _cameraManager.InitializeSingleCamera(CameraType.Fail, playerTransform, playerTransform);
            }
            else
            {
                var playerTransform = CurrentPlayer.transform;
                playerTransform.position = CurrentLevel.playerSpawnPoint.position;
                playerTransform.rotation = Quaternion.identity;
            }
        }

        public void StartLevel()
        {
            CurrentLevel._Start();
        }
        
    }
}