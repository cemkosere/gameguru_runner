using System;
using UnityEngine;

namespace Game.PlayerSystem
{
    public interface IPlayer
    {
        public void AddNewPoint(Vector3 point);
        public void Move();
        public void MovePoint(Vector3 point, Action callBack = null);
        public void Walk();
        public void Dance();
    }
}