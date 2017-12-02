using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : Tank {

    [SerializeField]
    private float MaxEngageDistance = 20f;
    [SerializeField]
    private float EngageClosestPropability = 1f;
    [SerializeField]
    private float EngageWeakestPropability = 1f;
    [SerializeField]
    private float EngageMostDangerousPropability = 1f;


    //-----------------------------Unity Functions-----------------------------

    private void FixedUpdate()
    {
        var playerTanks = GetPlayerTanks();
        var visiblePlayerTanks = GetVisibleTanksFrom(playerTanks);
        var visiblePlayerTanksInRange = GetTanksInRangeFrom(visiblePlayerTanks);        

        // If the current target is in range and visible the continue engaging.
        if (visiblePlayerTanksInRange.Count > 0 &&
           Target.pState == TankTarget.TargetState.Trans &&
           visiblePlayerTanksInRange.Contains(Target.pTargetTransform.GetComponent<Tank>()))
        {
            EngageTank(Target.pTargetTransform.GetComponent<Tank>());
            return;
        }

        // If the tank is being engaged then engage back.
        Tank engagingTank;
        if (IsBeingEngaged(out engagingTank))
        {
            EngageTank(engagingTank);
            return;
        }

        // If no tanks are within the maximum engagement distance then don't bother looking for a target.
        if (visiblePlayerTanks.Count <= 0 ||
            Vector3.Distance(GetClosestTankFrom(visiblePlayerTanks).transform.position, TurretTrans.position) > MaxEngageDistance)
        {
            EngageTank(null);
            return;
        }

        // Weighted randomely pick either the closest, weakenst or most dangerous tanks and engage.
        float probabilityTotal = EngageClosestPropability + EngageWeakestPropability + EngageMostDangerousPropability;
        float value = Random.Range(0f, probabilityTotal);
        if (value < EngageClosestPropability)
            EngageTank(GetClosestTankFrom(playerTanks)); // Engage closest
        else if (value < EngageClosestPropability + EngageWeakestPropability)
            EngageTank(GetWeakestTankFrom(playerTanks)); // Engage weakest
        else
            EngageTank(GetMostDangerousTankFrom(playerTanks)); // Engage most dangerous
    }


    //----------------------------Private Functions----------------------------

    private void EngageTank(Tank tank)
    {
        if (tank == null)
        {
            SetDestination(transform.position);
            return;
        }

        List<Tank> tankAsList = new List<Tank>() { tank };

        Target.SetTargetByTransform(tank.transform);

        if(GetVisibleTanksFrom(tankAsList).Count > 0 && GetTanksInRangeFrom(tankAsList).Count > 0)
            SetDestination(transform.position);
        else
            SetDestination(tank.transform.position);
    }

    private List<Tank> GetPlayerTanks()
    {
        List<Tank> playerTanks = new List<Tank>();
        foreach (var tank in FindObjectsOfType<Tank>())
            if (tank.pFaction == Faction.Player)
                playerTanks.Add(tank);
        return playerTanks;
    }

    private List<Tank> GetVisibleTanksFrom(List<Tank> tanks)
    {
        List<Tank> visibleTanks = new List<Tank>();
        foreach (var tank in tanks)
        {
            var turretToPos = tank.transform.position - TurretTrans.position;
            RaycastHit hit;
            if (Physics.Raycast(TurretTrans.position, turretToPos, out hit))
                if (hit.transform == tank.transform)
                    visibleTanks.Add(tank);
        }
        return visibleTanks;
    }

    private List<Tank> GetTanksInRangeFrom(List<Tank> tanks)
    {
        List<Tank> tanksInRange = new List<Tank>();
        foreach (var tank in tanks)
            if (Vector3.Distance(TurretTrans.position, tank.transform.position) <= pRange)
                tanksInRange.Add(tank);
        return tanksInRange;
    }

    private Tank GetClosestTankFrom(List<Tank> tanks)
    {
        if (tanks.Count <= 0) return null;
        Tank closestTank = tanks[0];
        float closestDistance = float.PositiveInfinity;
        foreach(var tank in tanks)
        {
            var distToTank = Vector3.Distance(TurretTrans.position, tank.transform.position);
            if (distToTank < closestDistance)
            {
                closestTank = tank;
                closestDistance = distToTank;
            }
        }
        return closestTank;
    }

    private Tank GetWeakestTankFrom(List<Tank> tanks)
    {
        if (tanks.Count <= 0) return null;
        Tank weakestTank = tanks[0];
        foreach (var tank in tanks)
            if (tank.pHealth < weakestTank.pHealth)
                weakestTank = tank;
        return weakestTank;
    }

    private Tank GetMostDangerousTankFrom(List<Tank> tanks)
    {
        if (tanks.Count <= 0) return null;
        Tank mostDangerousTank = tanks[0];
        foreach (var tank in tanks)
            if (tank.pDamagePerShot > mostDangerousTank.pDamagePerShot)
                mostDangerousTank = tank;
        return mostDangerousTank;
    }
}
