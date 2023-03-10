using System;
using System.Threading.Tasks;
using Ui.Views;
using UnityEngine;
using Zenject;

namespace Management
{
    public class GameManager : MonoBehaviour
    {
        private LevelManager _levelManager;
        private CameraManager _cameraManager;

        [Inject]
        private void OnInject(LevelManager levelManager, CameraManager cameraManager)
        {
            _levelManager = levelManager;
            _cameraManager = cameraManager;
            
            GameActions.LevelEnd += LevelEnd;
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            GameActions.OnTap -= Initialize;
            _levelManager.InitializeNewLevel();
            View.ChangeView(ViewType.MainMenu);
            _cameraManager.SetCamera(CameraType.MainMenu);
            GameActions.OnTap += LevelStart;
        }

        private void LevelStart()
        {
            GameActions.OnTap -= LevelStart;
            View.ChangeView(ViewType.GamePlay);
            _cameraManager.SetCamera(CameraType.GamePlay);
            _levelManager.StartLevel();
        }

        private void LevelEnd(bool isSuccess)
        {
            if (isSuccess)
            {
                Success();
            }
            else
            {
                Fail();
            }
        }

        private void Success()
        {
            _levelManager.CurrentLevelCompleted();
            _cameraManager.SetCamera(CameraType.Success);
            GameActions.OnTap += Initialize;
        }
        
        private async void Fail()
        {
            _cameraManager.SetCamera(CameraType.Fail);
            _cameraManager.InitializeSingleCamera(CameraType.Fail, null, null);
            await Task.Delay(TimeSpan.FromSeconds(1));
            _levelManager.RemoveLastLevel();
            Initialize();
        }
    }
}
