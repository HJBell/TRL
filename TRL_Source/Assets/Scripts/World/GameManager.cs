using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {


    //-----------------------------Unity Functions-----------------------------

    private void Update()
    {
        
    }


    //----------------------------Public Functions-----------------------------

    public void StartBattle()
    {
        foreach (var spawnPoint in FindObjectsOfType<SpawnPoint>())
            spawnPoint.OnStartBattle();

        // Bake nav mesh.

        Destroy(FindObjectOfType<UI_Deck>().gameObject);

        FindObjectOfType<UI_PauseButton>().SetPause(false);

        GameInfo.State = GameState.Battle;
    }
}

