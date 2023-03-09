using UnityEngine;
using Zenject;

namespace Management
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip boingClip;
        private AudioSource _audioSource;

        public static AudioManager Instance;
        
        [Inject]
        private void OnInject()
        {
            _audioSource = GetComponent<AudioSource>();
            Instance = this;
        }

        private int _perfectCount = 0;
        public void PerfectHit()
        {
            _audioSource.PlayOneShot(boingClip);
            _perfectCount++;
            _audioSource.pitch = 1 + (_perfectCount * .2f);
        }

        public void ResetHit()
        {
            if(_perfectCount == 0) return;
            _perfectCount = 0;
            _audioSource.pitch = 1;
        }
        
        
    }
}