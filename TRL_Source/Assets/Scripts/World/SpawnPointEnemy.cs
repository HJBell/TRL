using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointEnemy : SpawnPoint {

    [SerializeField]
    private Tank TankToSpawn;


    //----------------------------Public Functions-----------------------------

    public override Faction GetFaction()
    {
        return Faction.Enemy;
    }

    public override void OnStartBattle()
    {
        var obj = Instantiate(TankToSpawn.gameObject, transform.position, transform.rotation) as GameObject;
        obj.GetComponent<Tank>().Faction = Faction.Enemy;
        Destroy(this.gameObject);
    }
}
