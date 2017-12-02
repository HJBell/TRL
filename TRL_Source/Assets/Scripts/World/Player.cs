using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private Transform SelectionHighlighterTrans;
    [SerializeField]
    private LineOfSight LineOfSightIndicator;

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
        bool tankIsSelected = mSelectedTank != null;

        SelectionHighlighterTrans.gameObject.SetActive(tankIsSelected);
        LineOfSightIndicator.gameObject.SetActive(tankIsSelected);

        if (!tankIsSelected) return;

        var newPos = mSelectedTank.transform.position;

        // Highlight tanks.
        newPos.y = SelectionHighlighterTrans.position.y;
        SelectionHighlighterTrans.position = newPos;

        // Indicate line of sight.
        newPos.y = LineOfSightIndicator.transform.position.y;
        LineOfSightIndicator.transform.position = newPos;
        LineOfSightIndicator.Range = mSelectedTank.pRange;
        LineOfSightIndicator.DrawFieldOfView();
    }


    //----------------------------Private Functions----------------------------

    private void SetSelectedTank(Tank tank)
    {
        mSelectedTank = tank;
    }
}
