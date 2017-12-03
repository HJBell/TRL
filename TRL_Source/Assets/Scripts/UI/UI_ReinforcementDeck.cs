using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ReinforcementDeck : UI_Deck {

    [System.Serializable]
    struct ReinforcementCard
    {
        public UI_Card Card;
        public float Probability;
    }

    [SerializeField]
    private List<ReinforcementCard> ReinforcementCards = new List<ReinforcementCard>();


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

        float totalProbability = 0f;
        foreach (var card in ReinforcementCards)
            totalProbability += card.Probability;

        float probabilityValue = Random.Range(0f, totalProbability);

        float probabilitySum = 0f;
        foreach(var card in ReinforcementCards)
        {
            if (probabilityValue <= probabilitySum + card.Probability)
            {
                InitialCardsInDeck.Add(card.Card);
                break;
            }
            else
                probabilitySum += card.Probability;
        }

        SpawnInitialCards();
    }
}
