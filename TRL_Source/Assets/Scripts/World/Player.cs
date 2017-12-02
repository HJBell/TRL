using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private Transform SelectionHighlighterTrans;

    private Tank mSelectedTank;


    //-----------------------------Unity Functions-----------------------------

    private void Update()
    {
        // LMB.
        if(Input.GetMouseButtonDown(0))
        {
            var mouseScreenPos = Input.mousePosition;
            var mousePosRay = Camera.main.ScreenPointToRay(mouseScreenPos);
            RaycastHit hit;

            if (Physics.Raycast(mousePosRay, out hit))
            {
                // LMB on tank.
                if(hit.collider.GetComponent<Tank>())
                {
                    var tank = hit.collider.GetComponent<Tank>();
                    if (tank.pFaction == Faction.Player)
                        SetSelectedTank(tank);
                }

                // LMB on non tank.
                else
                    SetSelectedTank(null);
            }
        }

        // RMB.
        if (Input.GetMouseButtonDown(1))
        {
            var mouseScreenPos = Input.mousePosition;
            var mousePosRay = Camera.main.ScreenPointToRay(mouseScreenPos);
            RaycastHit hit;

            if (Physics.Raycast(mousePosRay, out hit))
            {
                // RMB on tank.
                if (hit.collider.GetComponent<Tank>())
                {
                    var tank = hit.collider.GetComponent<Tank>();
                    if (tank.pFaction == Faction.Enemy)
                        if (mSelectedTank != null)
                            mSelectedTank.Target.SetTargetByTransform(tank.transform);
                }

                // RMB on non tank.
                else if (mSelectedTank != null)                    
                        mSelectedTank.SetDestination(hit.point);
            }
        }
    }

    private void LateUpdate()
    {
        SelectionHighlighterTrans.gameObject.SetActive(mSelectedTank != null);

        if (mSelectedTank == null) return;

        var newHighlightPos = mSelectedTank.transform.position;
        newHighlightPos.y = SelectionHighlighterTrans.position.y;
        SelectionHighlighterTrans.position = newHighlightPos;
    }


    //----------------------------Private Functions----------------------------

    private void SetSelectedTank(Tank tank)
    {
        mSelectedTank = tank;
    }
}
