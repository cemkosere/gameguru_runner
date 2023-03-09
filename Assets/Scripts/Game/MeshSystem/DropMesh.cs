using System;
using System.Threading.Tasks;
using Game.Pools;
using UnityEngine;

namespace Game.MeshSystem
{
    public class DropMesh : ScalableMesh
    {
        [SerializeField] private Rigidbody rBody;
        
        public override void SetScaleAndPosition(Vector3 scale, Vector3 position)
        {
            rBody.isKinematic = true;
            transform.rotation = Quaternion.identity;
            Scale = scale;
            Position = position;
            gameObject.SetActive(true);
            rBody.isKinematic = false;
        }

        private async void OnEnable()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            DropMeshPool.Instance.Return(this);
        }
    }
}