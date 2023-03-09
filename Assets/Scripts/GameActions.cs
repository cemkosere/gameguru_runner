using UnityEngine.Events;

public static class GameActions
{
    public static UnityAction OnTap = delegate {  };
    
    public static UnityAction<bool> LevelEnd = delegate {  };
}