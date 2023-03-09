using UnityEngine;

namespace Game.MeshSystem
{
    public abstract class ScalableMesh : MonoBehaviour
    {
        private Transform _transform;
        public Transform Transform
        {
            get
            {
                if (!_transform)
                    _transform = transform;
                return _transform;
            }
        }
        public Vector3 Scale
        {
            get => Transform.localScale;
            set => Transform.localScale = value;
        }
        public Vector3 Position
        {
            get => Transform.position;
            set => Transform.position = value;
        }

        public abstract void SetScaleAndPosition(Vector3 scale, Vector3 position);
    }
}