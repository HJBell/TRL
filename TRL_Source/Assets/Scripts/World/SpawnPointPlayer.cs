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
            var obj = Instantiate(mTank.Card.gameObject) as GameObject;
            obj.transform.SetParent(FindObjectOfType<UI_Deck>().transform);
            obj.transform.position = Input.mousePosition;
            obj.GetComponent<UI_Card>().StartCustomDrag();

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
    }

    public override void OnStartBattle()
    {
        if (mTank)
            mTank.SetTankNumber(Number);
        Destroy(this.gameObject);
    }
}
