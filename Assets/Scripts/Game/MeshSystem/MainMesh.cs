using UnityEngine;

namespace Game.MeshSystem
{
    public class MainMesh : ScalableMesh
    {
        public Vector3 endPoint => Position + Vector3.forward * Scale.z;
        
        public override void SetScaleAndPosition(Vector3 scale, Vector3 position)
        {
            Scale = scale;
            Position = position;
            
            
        }
    }
}