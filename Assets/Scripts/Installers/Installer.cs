using Management;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class Installer : MonoInstaller
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private CameraManager cameraManager;
        
        
        public override void InstallBindings()
        {
            Container.BindInstance(gameManager).AsSingle();
            Container.BindInstance(levelManager).AsSingle();
            Container.BindInstance(cameraManager).AsSingle();
        }
    }
}