using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointPlayer : SpawnPoint {

    [SerializeField]
    private int Number = 0;

    public bool pHasTank { get { return mTank != null; } }

    private Tank mTank;
    private bool mIsBeingDragged = false;


    //----------------------------------------Unity Functions----------------------------------------

    private void OnMouseDown()
    {
        mIsBeingDragged = true;
    }

    private void OnMouseUp()
    {
        mIsBeingDragged = false;
    }

    private void OnMouseExit()
    {
        if (mIsBeingDragged && mTank != null)
        {
            var card = FindObjectOfType<UI_StartDeck>().SpawnLooseCardAt(mTank.Card, Input.mousePosition);
            card.StartCustomDrag();

            Destroy(mTank.gameObject);
        }
    }



    //----------------------------Public Functions-----------------------------

    public override Faction GetFaction()
    {
        return Faction.Player;
    }

    public void SpawnTank(Tank tank)
    {
        mTank = (Instantiate(tank.gameObject, transform.position, transform.rotation) as GameObject).GetComponent<Tank>();

        var obj = Instantiate(Resources.Load("Misc/Res_AudioSource")) as GameObject;
        obj.GetComponent<AudioSourceManager>().PlayEquip();
    }

    public override void OnStartBattle()
    {
        if (mTank)
            mTank.SetTankNumber(Number);
        Destroy(this.gameObject);
    }
}
