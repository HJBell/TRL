using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTarget {

    public enum TargetState
    {
        Trans, Pos, None
    }

    public TargetState pState { get { UpdateState(); return mState; } }
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

        UpdateState();

        if (mState == TargetState.None) return false;
        if (mState == TargetState.Trans) targetPos = mTargetTransform.position;
        
        return true;
    }


    //----------------------------Private Functions----------------------------

    private void UpdateState()
    {
        // Checking if the target trans has become null.
        if (mState == TargetState.Trans && mTargetTransform == null)
            ClearTarget();
    }    
}
