using DG.Tweening;
using Game.Pools;
using Management;
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
        
        public Tween StartMovement()
        {
            return Transform
                .DOMoveX(-6, GameConstants.RoadPieceSpeed)
                .SetSpeedBased(true)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }
        
        
        
        public bool CalculateScale(MainMesh lastMesh)
        {
            var diff = lastMesh.Position.x - Position.x;
            var absDiff = Mathf.Abs(diff);
            if (absDiff < lastMesh.Scale.x *  GameConstants.ToleranceRatio)
            {//TOLERANCE
                Position = lastMesh.Position + Vector3.forward * lastMesh.Scale.z;
                //Perfect
                AudioManager.Instance.PerfectHit();
                return true;
            }
            AudioManager.Instance.ResetHit();
            var dropMesh = DropMeshPool.Instance.Rent();
            
            if (absDiff >= lastMesh.Scale.x)
            {
                dropMesh.SetScaleAndPosition(lastMesh.Scale, Position);
                MainMeshPool.Instance.Return(this);
                return false;
            }
            
            Scale -= Vector3.right * absDiff;
            Position += Vector3.right * (diff / 2);

            
            var x = ((Scale.x / 2) + (absDiff / 2) + Mathf.Abs(Position.x) ) * (diff < 0 ? 1 : -1);
            dropMesh.SetScaleAndPosition(
                new Vector3(absDiff,1,lastMesh.Scale.z), 
                new Vector3(x,Position.y,Position.z));
            return true;
        }
    }
}