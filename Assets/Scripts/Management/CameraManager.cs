using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace Management
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineBrain brain;
        [Space]
        [SerializeField] private CinemachineVirtualCamera mainMenuCamera;
        [SerializeField] private CinemachineVirtualCamera gamePlayCamera;
        [SerializeField] private CinemachineVirtualCamera successCamera;
        [SerializeField] private CinemachineVirtualCamera failCamera;

        private Dictionary<CameraType, CinemachineVirtualCamera> _cameraDict;
        private CameraType _currentCameraType;

        [Inject]
        private void OnInject()
        {
            Initialize();
        }

        public void Initialize()
        {
            _cameraDict = new Dictionary<CameraType, CinemachineVirtualCamera>
            {
                {CameraType.MainMenu, mainMenuCamera},
                {CameraType.GamePlay, gamePlayCamera},
                {CameraType.Success, successCamera},
                {CameraType.Fail, failCamera},
            };
        }
        
        public void InitializeSingleCamera(CameraType cameraType, Transform follow = null, Transform lookAt = null)
        {
            var cam =_cameraDict[cameraType]; 
            cam.Follow = follow;
            cam.LookAt = lookAt;
        }

        public void SetCamera(CameraType cameraType)
        {
            if(_currentCameraType == cameraType) return;
            _cameraDict[_currentCameraType].gameObject.SetActive(false);
            _currentCameraType = cameraType;
            _cameraDict[_currentCameraType].gameObject.SetActive(true);
        }

        
    }
    
    public enum CameraType
    {
        MainMenu,
        GamePlay,
        Success,
        Fail
    
    }
}