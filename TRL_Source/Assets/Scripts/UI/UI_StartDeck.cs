using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StartDeck : UI_Deck {

    [SerializeField]
    private Button StartBattleButton;
    [SerializeField]
    private UI_WinDeck WinDeck;


    //-----------------------------Unity Functions-----------------------------

    protected override void Start()
    {
        base.Start();

        InitialCardsInDeck.Clear();

        // Load deck.
        for (int i = 0; i < 4; i++)
        {
            string cardString = PlayerPrefs.GetString("Card" + i);
            if (cardString != "")
            {
                var obj = Resources.Load(cardString) as GameObject;
                InitialCardsInDeck.Add(obj.GetComponent<UI_Card>());
            }    
        }

        SpawnInitialCards();
    }

    protected override void Update()
    {
        StartBattleButton.interactable = false;
        foreach (var spawnPoint in FindObjectsOfType<SpawnPointPlayer>())
            if (spawnPoint.pHasTank)
                StartBattleButton.interactable = true;
        base.Update();
    }


    //----------------------------Public Functions-----------------------------

    public void MoveRemainingCardsToWinDeck()
    {
        if (WinDeck == null) return;

        foreach (var card in mCardsInSlots)
        {
            foreach (var cardPrefab in InitialCardsInDeck)
            {
                if (card.Tank == cardPrefab.Tank)
                {
                    WinDeck.InitialCardsInDeck.Add(cardPrefab);
                    break;
                }
            }
        }
    }
}
