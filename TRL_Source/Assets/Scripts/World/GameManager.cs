using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private GameObject WinScreen;
    [SerializeField]
    private GameObject LooseScreen;
    [SerializeField]
    private string NextLevelString;


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

        FindObjectOfType<UI_StartDeck>().MoveRemainingCardsToWinDeck();
        Destroy(FindObjectOfType<UI_StartDeck>().gameObject);

        FindObjectOfType<UI_PauseButton>().SetPause(false);

        GameInfo.State = GameState.Battle;
    }

    public void WinBattle()
    {
        EndBattle();
        WinScreen.SetActive(true);
        FindObjectOfType<UI_WinDeck>().Init();
        FindObjectOfType<UI_ReinforcementDeck>().Init();
    }

    public void LooseBattle()
    {
        EndBattle();
        LooseScreen.SetActive(true);
    }

    public void LoadNextBattle()
    {
        FindObjectOfType<UI_WinDeck>().SaveCardsInDeck();
        SceneManager.LoadScene(NextLevelString);
    }


    //----------------------------Private Functions----------------------------

    private void EndBattle()
    {
        GameInfo.State = GameState.Postbattle;
    }
}

