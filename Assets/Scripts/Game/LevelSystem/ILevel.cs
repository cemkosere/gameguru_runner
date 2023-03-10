using Game.PlayerSystem;

namespace Game.LevelSystem
{
    public interface ILevel
    {
        public void Initialize(IPlayer player);
        public void InitializeRoad();
        public void _Start();
        public void StartLoop();
        public void OnTap();
    }
}