using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnPoint : MonoBehaviour {


    //----------------------------Public Functions-----------------------------

    public abstract Faction GetFaction();
    public abstract void OnStartBattle();
}
