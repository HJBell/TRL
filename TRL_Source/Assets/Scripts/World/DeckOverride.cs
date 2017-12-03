using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckOverride : MonoBehaviour {

    [SerializeField]
    private bool Override = false;
    [SerializeField]
    private List<string> Cards = new List<string>();


    //-----------------------------Unity Functions-----------------------------

    private void Awake()
    {
        if (Override)
            OverrideDeck();
    }


    //----------------------------Public Functions-----------------------------

    public void OverrideDeck()
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerPrefs.SetString("Card" + i, "");
            if(i < Cards.Count)
                PlayerPrefs.SetString("Card" + i, Cards[i]);
        }
    }
}
