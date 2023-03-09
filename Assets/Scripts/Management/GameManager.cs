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
            Initialize();
            GameActions.LevelEnd += LevelEnd;
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
            _cameraManager.SetCamera(CameraType.Success);
            GameActions.OnTap += Initialize;
        }
        
        private void Fail()
        {
            _cameraManager.SetCamera(CameraType.Fail);
            //LevelManager'
        }
    }
}
