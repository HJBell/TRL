using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WinDeck : UI_Deck {


    //-----------------------------Unity Functions-----------------------------

    protected override void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();
    }


    //----------------------------Public Functions-----------------------------

    public void Init()
    {
        GenerateRandomSeed();

        foreach (var tank in FindObjectsOfType<Tank>())
            if (tank.Faction == Faction.Player)
                InitialCardsInDeck.Add(tank.Card);

        SpawnInitialCards();
    }
}
