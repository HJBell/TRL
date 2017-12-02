﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : PathFinder {

    [HideInInspector]
    public TankTarget Target = new TankTarget();

    [SerializeField]
    private int MaxHealth = 10;
    [SerializeField]
    private float ReloadDuration = 1f;
    [SerializeField]
    private int DamagePerShot = 2;
    [SerializeField]
    private float Range = 10f;
    [SerializeField]
    private float TurretRotSpeed = 90f;
    [SerializeField]
    private float MinFireAngle = 5f;
    [SerializeField]
    private Transform TurretTrans;
    [SerializeField]
    private Transform ProjectileRaySource;

    private int mHealth = 0;
    private float mTimeOfLastShot = 0f;
    private UIBar mHealthBar;
    private UIBar mReloadBar;

    private float pReloadProgress { get { return Mathf.Clamp(Time.time - mTimeOfLastShot, 0f, ReloadDuration); } }


    //-----------------------------Unity Functions-----------------------------

    private void Awake()
    {
        mHealth = MaxHealth;
        mTimeOfLastShot = Time.time - ReloadDuration;

        mHealthBar = (Instantiate(Resources.Load("Res_HealthBar")) as GameObject).GetComponent<UIBar>();
        mHealthBar.transform.SetParent(FindObjectOfType<Canvas>().transform);
        mReloadBar = (Instantiate(Resources.Load("Res_ReloadBar")) as GameObject).GetComponent<UIBar>();
        mReloadBar.transform.SetParent(FindObjectOfType<Canvas>().transform);
    }

    private void Update()
    {
        // Updating the tank UI.
        UpdateUI();

        // Checking if the tank has a target.
        Vector3 targetPos = Vector3.zero;
        if (!Target.HasTarget(out targetPos))
            return;

        // If the target pos is vibile from the turret...
        if(TargetVisibleFromTurret())
        {
            // Rotating the turret to face the target.
            RotateTurretToFace(targetPos);

            // Firing if aiming at target and target in range.
            bool aimingAtTarget = GetTurretAngleToPos(targetPos) <= MinFireAngle;
            bool targetInRange = Vector3.Distance(ProjectileRaySource.position, targetPos) <= Range;
            if (aimingAtTarget && targetInRange) Fire();
        }        
    }

    private void OnDestroy()
    {
        if (mHealthBar) Destroy(mHealthBar.gameObject);
        if (mReloadBar) Destroy(mReloadBar.gameObject);
    }


    //----------------------------Public Functions-----------------------------

    public void TakeDamage(int damage)
    {
        mHealth -= damage;
        if (mHealth <= 0) DestroyImmediate(this.gameObject);
    }


    //---------------------------Protected Functions---------------------------

    protected void Fire()
    {
        if (pReloadProgress < ReloadDuration) return;
        FireImmediate();
        mTimeOfLastShot = Time.time;
    }   


    //----------------------------Private Functions----------------------------

    private void UpdateUI()
    {
        mHealthBar.SetWorldPos(transform.position);
        mHealthBar.SetValue(mHealth, MaxHealth);
        mReloadBar.SetWorldPos(transform.position);
        mReloadBar.SetValue(pReloadProgress, ReloadDuration);
    }

    private void FireImmediate()
    {
        Vector3 projectileEndPos = ProjectileRaySource.position + TurretTrans.forward * Range;

        RaycastHit hit;
        if (Physics.Raycast(ProjectileRaySource.position, TurretTrans.forward, out hit, Range))
        {
            projectileEndPos = hit.point;
            if (hit.collider.GetComponent<Tank>())
                hit.collider.GetComponent<Tank>().TakeDamage(DamagePerShot);
        }

        var obj = Instantiate(Resources.Load("Res_ProjectileTrail")) as GameObject;
        obj.GetComponent<ProjectileTrail>().Init(ProjectileRaySource.position, projectileEndPos);
    }

    private void RotateTurretToFace(Vector3 pos)
    {
        pos.y = TurretTrans.position.y;
        var targetRotation = Quaternion.LookRotation(pos - TurretTrans.position);
        TurretTrans.rotation = Quaternion.Lerp(TurretTrans.rotation, targetRotation, TurretRotSpeed * Mathf.Deg2Rad * Time.deltaTime);
    }

    private float GetTurretAngleToPos(Vector3 pos)
    {
        pos.y = TurretTrans.position.y;
        return Vector3.Angle(TurretTrans.forward, pos - TurretTrans.position);
    }

    private bool TargetVisibleFromTurret()
    {
        Vector3 targetPos = Vector3.zero;
        if (!Target.HasTarget(out targetPos))
            return false;

        var turretToPos = targetPos - TurretTrans.position;
        RaycastHit hit;
        if (Physics.Raycast(TurretTrans.position, turretToPos, out hit))
        {
            if (Target.pState == TankTarget.TargetState.Trans && hit.transform == Target.pTargetTransform)
                return true;    
            else if (hit.distance >= turretToPos.magnitude)
                return true;
        }
        return false;
    }
}