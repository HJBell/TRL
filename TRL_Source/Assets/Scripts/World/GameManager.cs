using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private GameObject WinScreen;
    [SerializeField]
    private GameObject LooseScreen;


    //-----------------------------Unity Functions-----------------------------

    private void Awake()
    {
        WinScreen.SetActive(false);
        LooseScreen.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (GameInfo.State != GameState.Battle) return;

        var tanks = FindObjectsOfType<Tank>();
        bool didWin = true;
        bool didLoose = true;
        foreach(var tank in tanks)
        {
            if (tank.Faction == Faction.Player) didLoose = false;
            if (tank.Faction == Faction.Enemy) didWin = false;
        }

        if (didLoose) LooseBattle();
        else if (didWin) WinBattle();
    }


    //----------------------------Public Functions-----------------------------

    public void StartBattle()
    {
        foreach (var spawnPoint in FindObjectsOfType<SpawnPoint>())
            spawnPoint.OnStartBattle();

        Destroy(FindObjectOfType<UI_Deck>().gameObject);

        FindObjectOfType<UI_PauseButton>().SetPause(false);

        GameInfo.State = GameState.Battle;
    }

    public void WinBattle()
    {
        EndBattle();
        WinScreen.SetActive(true);
    }

    public void LooseBattle()
    {
        EndBattle();
        LooseScreen.SetActive(true);
    }


    //----------------------------Private Functions----------------------------

    private void EndBattle()
    {
        GameInfo.State = GameState.Postbattle;
    }
}

