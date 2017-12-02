using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    PreBattle, Battle, PostBattle
}

public static class GameInfo {

    public static GameState State = GameState.PreBattle;
}
