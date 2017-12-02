using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointEnemy : SpawnPoint {

    [SerializeField]
    private string TankToSpawn = "Tanks/Res_TankEnemy";


    //----------------------------Public Functions-----------------------------

    public override Faction GetFaction()
    {
        return Faction.Enemy;
    }

    public override void OnStartBattle()
    {
        Instantiate(Resources.Load(TankToSpawn), transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
