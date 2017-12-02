using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTarget {

    public enum TargetState
    {
        Trans, Pos, None
    }

    public TargetState pState { get { return mState; } }
    public Transform pTargetTransform { get { return mTargetTransform; } }

    private TargetState mState = TargetState.None;
    private Transform mTargetTransform;
    private Vector3 mTargetPosition;


    //----------------------------Public Functions-----------------------------

    public void SetTargetByTransform(Transform trans)
    {
        mTargetTransform = trans;
        mState = TargetState.Trans;
    }

    public void SetTargetByPosition(Vector3 pos)
    {         
        mTargetPosition = pos;
        mState = TargetState.Pos;
    }

    public void ClearTarget()
    {
        mState = TargetState.None;
    }

    public bool HasTarget(out Vector3 targetPos)
    {
        targetPos = mTargetPosition;

        // Checking if the target trans has become null.
        if (mState == TargetState.Trans && mTargetTransform == null)
            ClearTarget();

        if (mState == TargetState.None) return false;
        if (mState == TargetState.Trans) targetPos = mTargetTransform.position;
        
        return true;
    }
}
