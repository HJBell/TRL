using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class PathFinder : MonoBehaviour {

    public Vector3 pDestination { get { return mDestination; } }

    private Vector3 mDestination;
    private NavMeshAgent mNavMeshAgent;


    //----------------------------Public Functions-----------------------------

    public void SetDestination(Vector3 pos)
    {
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
