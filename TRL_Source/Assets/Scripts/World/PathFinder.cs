using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class PathFinder : MonoBehaviour {

    private Vector3 mDestination;
    private NavMeshAgent mNavMeshAgent;


    //----------------------------Public Functions-----------------------------

    public void SetDestination(Vector3 pos)
    {
        if (Vector3.Distance(pos, transform.position) < 0.1f)
            return;
        mDestination = pos;
        UpdateNavMeshAgent();
    }


    //----------------------------Private Functions----------------------------

    private void UpdateNavMeshAgent()
    {
        if (mNavMeshAgent == null)
            mNavMeshAgent = GetComponent<NavMeshAgent>();
        mNavMeshAgent.SetDestination(mDestination);
    }
}
