using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    [SerializeField]
    private Transform SelectionHighlighterTrans;
    [SerializeField]
    private LineOfSight LineOfSightIndicator;
    [SerializeField]
    private Transform DestinationIndicatorTrans;
    [SerializeField]
    private Image TargetIndicatorImage;

    private Tank mSelectedTank;


    //-----------------------------Unity Functions-----------------------------

    private void Update()
    {
        if (GameInfo.State != GameState.Battle) return;

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
                    if (tank.Faction == Faction.Player)
                        SetSelectedTank(tank);
                }

                // LMB on non tank.
                else
                    SetSelectedTank(null);
            }
        }

        // RMB.
        if (Input.GetMouseButton(1))
        {
            var mouseScreenPos = Input.mousePosition;
            var mousePosRay = Camera.main.ScreenPointToRay(mouseScreenPos);
            RaycastHit hit;

            if (Physics.Raycast(mousePosRay, out hit))
            {
                // RMB on tank.
                if (hit.collider.GetComponent<Tank>() && Input.GetMouseButtonDown(1))
                {
                    var tank = hit.collider.GetComponent<Tank>();
                    if (tank.Faction == Faction.Enemy)
                        if (mSelectedTank != null)
                            mSelectedTank.Target.SetTargetByTransform(tank.transform);
                }

                // RMB on non tank.
                else if (mSelectedTank != null && hit.collider.tag == "Ground")                    
                    mSelectedTank.SetDestination(hit.point);
            }
        }

        // Number hotkeys for tanks.
        var tanks = FindObjectsOfType<Tank>();
        List<Tank> playerTanks = new List<Tank>();
        foreach (var tank in tanks)
            if (tank.Faction == Faction.Player)
                playerTanks.Add(tank);
        for(int i = 1; i <= 9; i++)
            if(Input.GetKeyDown(i.ToString()))
                foreach (var tank in playerTanks)
                    if (tank.pNumber == i)
                        mSelectedTank = tank;
    }

    private void LateUpdate()
    {
        bool tankIsSelected = mSelectedTank != null;

        SelectionHighlighterTrans.gameObject.SetActive(tankIsSelected);
        LineOfSightIndicator.gameObject.SetActive(tankIsSelected);
        DestinationIndicatorTrans.gameObject.SetActive(tankIsSelected);
        TargetIndicatorImage.gameObject.SetActive(tankIsSelected);

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

        // Indicate destination.
        newPos = mSelectedTank.pDestination;
        newPos.y = DestinationIndicatorTrans.position.y;
        DestinationIndicatorTrans.position = newPos;
        if (Vector3.Distance(mSelectedTank.transform.position, newPos) < 2f)
            DestinationIndicatorTrans.gameObject.SetActive(false);

        // Indicate target.
        Vector3 targetPos = Vector3.zero;
        if (mSelectedTank.Target.HasTarget(out targetPos))
        {
            TargetIndicatorImage.transform.SetAsFirstSibling();
            var screenPos = Camera.main.WorldToScreenPoint(targetPos);
            TargetIndicatorImage.transform.position = screenPos;
        }
        else
            TargetIndicatorImage.gameObject.SetActive(false);
    }


    //----------------------------Private Functions----------------------------

    private void SetSelectedTank(Tank tank)
    {
        mSelectedTank = tank;
    }
}
