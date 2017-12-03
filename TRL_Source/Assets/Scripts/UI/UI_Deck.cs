using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UI_Deck : MonoBehaviour {

    [HideInInspector]
    public List<UI_Card> InitialCardsInDeck = new List<UI_Card>();
    
    [SerializeField]
    protected List<Transform> Slots = new List<Transform>();

    protected List<UI_Card> mCardsInSlots = new List<UI_Card>();

    private int mRandomSeed = 0;


    //-----------------------------Unity Functions-----------------------------

    protected virtual void Start()
    {
        GenerateRandomSeed();
    }

    protected virtual void Update()
    {

    }


    //----------------------------Public Functions-----------------------------

    public UI_Card SpawnLooseCardAt(UI_Card card, Vector3 pos)
    {
        var obj = Instantiate(card.gameObject) as GameObject;
        obj.transform.SetParent(this.transform);
        obj.transform.SetAsLastSibling();
        obj.transform.position = pos;
        obj.GetComponent<UI_Card>().SetDeck(this);
        return obj.GetComponent<UI_Card>();
    }

    public void UpdateDeck()
    {
        for (int i = 0; i < mCardsInSlots.Count; i++)
        {
            mCardsInSlots[i].transform.position = Slots[i].transform.position;
            var rot = mCardsInSlots[i].transform.eulerAngles;
            Random.InitState(mRandomSeed + i);
            rot.z = Random.Range(-5f, 5f);
            mCardsInSlots[i].transform.eulerAngles = rot;
        }
        Random.InitState((int)(Time.time * 99999f));
    }

    public void RemoveCard(UI_Card card)
    {
        if (!mCardsInSlots.Contains(card)) return;
        mCardsInSlots.Remove(card);
        UpdateDeck();
    }

    public void AddCard(UI_Card card)
    {
        if (mCardsInSlots.Count >= Slots.Count) return;
        if (mCardsInSlots.Contains(card)) return;
        mCardsInSlots.Add(card);
        card.SetDeck(this);
        UpdateDeck();
    }

    public UI_Card AddCardAtIndex(UI_Card card, int index)
    {
        if (mCardsInSlots.Contains(card)) return null;

        UI_Card cardCurrentlyAtIndex = null;
        if (index < mCardsInSlots.Count)
        {
            cardCurrentlyAtIndex = mCardsInSlots[index];
            RemoveCard(cardCurrentlyAtIndex);
            mCardsInSlots.Insert(index, card);
            card.SetDeck(this);
        }
        else
            AddCard(card);
        UpdateDeck();

        return cardCurrentlyAtIndex;
    }

    public bool MouseIsOverSlot(out int index)
    {
        var mousePos = Input.mousePosition;

        for(int i = 0; i < Slots.Count; i++)
        {
            index = i;

            Vector2 pos = new Vector2(Slots[i].transform.position.x, Slots[i].transform.position.y);
            Vector2 dims = new Vector2(Slots[i].GetComponent<RectTransform>().rect.width, Slots[i].GetComponent<RectTransform>().rect.height);

            if (mousePos.x < pos.x + dims.x / 2f && mousePos.x > pos.x - dims.x / 2f)
                if (mousePos.y < pos.y + dims.y / 2f && mousePos.y > pos.y - dims.y / 2f)
                    return true;
        }
        index = 0;
        return false;
    }

    public void SaveCardsInDeck()
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerPrefs.SetString("Card" + i, "");
            if(i < mCardsInSlots.Count)
            {
                var cardPath = mCardsInSlots[i].pPath;
                PlayerPrefs.SetString("Card" + i, cardPath);
            }
        }
    }


    //---------------------------Protected Functions---------------------------

    protected void SpawnInitialCards()
    {
        foreach (var card in InitialCardsInDeck)
        {
            var cardObj = SpawnLooseCardAt(card, Vector3.zero);
            mCardsInSlots.Add(cardObj);
        }

        UpdateDeck();
    }

    protected void GenerateRandomSeed()
    {
        mRandomSeed = (int)(Time.realtimeSinceStartup * 99999f);
    }
}
