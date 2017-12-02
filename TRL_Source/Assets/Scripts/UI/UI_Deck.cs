using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Deck : MonoBehaviour {

    [SerializeField]
    private Button StartBattleButton;
    [SerializeField]
    private List<Transform> Slots = new List<Transform>();
    [SerializeField]
    private List<UI_Card> CardsInDeck = new List<UI_Card>();

    private List<UI_Card> mCardObjs = new List<UI_Card>();


    //-----------------------------Unity Functions-----------------------------

    private void Start()
    {
        // Spawn cards UI objs.
        foreach(var card in CardsInDeck)
        {
            var obj = Instantiate(card.gameObject) as GameObject;
            mCardObjs.Add(obj.GetComponent<UI_Card>());
            obj.transform.SetParent(this.transform);
            obj.transform.SetAsLastSibling();
        }

        UpdateDeck();
    }

    private void Update()
    {
        StartBattleButton.interactable = false;
        foreach (var spawnPoint in FindObjectsOfType<SpawnPointPlayer>())
            if (spawnPoint.pHasTank)
                StartBattleButton.interactable = true;

    }


    //----------------------------Public Functions-----------------------------

    public void UpdateDeck()
    {
        for (int i = 0; i < mCardObjs.Count; i++)
            mCardObjs[i].transform.position = Slots[i].transform.position;
    }

    public void RemoveCard(UI_Card card)
    {
        if (!mCardObjs.Contains(card)) return;
        mCardObjs.Remove(card);
        UpdateDeck();
    }

    public void AddCard(UI_Card card)
    {
        if (mCardObjs.Contains(card)) return;
        mCardObjs.Add(card);
        UpdateDeck();
    }
}
