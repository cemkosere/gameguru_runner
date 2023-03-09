using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Helpers
{
    public class Pool<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] protected T prefab;
        [SerializeField] protected int count;

        private Queue<T> _insQueue;

        public static Pool<T> Instance;

        [Inject]
        private void OnInject()
        {
            Initialize();
        }

        private void Initialize()
        {
            Instance = this;
            _insQueue = new Queue<T>();
            for (int i = 0; i < count; i++)
            {
                var insPrefab = Instantiate(prefab, transform);
                _insQueue.Enqueue(insPrefab);
                insPrefab.gameObject.SetActive(false);
            }
        }

        public T Rent()
        {
            return _insQueue.Count < 1 ? Instantiate(prefab, transform) : _insQueue.Dequeue();
        }

        public void Return(T insObj)
        {
            insObj.gameObject.SetActive(false);
            _insQueue.Enqueue(insObj);
        }
    }
}