using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction
{
    Player, Enemy
}

public class Tank : PathFinder {

    public UI_Card Card;
    public Transform TurretTrans;
    public Faction Faction;

    [HideInInspector]
    public TankTarget Target = new TankTarget();

    public float pRange { get { return Range; } }
    public int pHealth { get { return mHealth; } }
    public int pDamagePerShot { get { return DamagePerShot; } }
    public int pNumber { get { return mNumber; } }

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
    private Transform ProjectileRaySource;
    [SerializeField]
    private List<TextMesh> TankNumberMeshes;
    [SerializeField]
    private Color PlayerColour = Color.yellow;
    [SerializeField]
    private Color EnemyColour = Color.red;
    [SerializeField]
    private List<MeshRenderer> TintedObjects;
    [SerializeField]
    private GameObject HealthBarPrefab;
    [SerializeField]
    private GameObject ReloadBarPrefab;

    private int mHealth = 0;
    private float mTimeOfLastShot = 0f;
    private UI_NodeBar mHealthBar;
    private UI_Bar mReloadBar;
    private float mTimeOfLastTakeDamage = float.NegativeInfinity;
    private Tank mTankThatLastDealtDamage;
    private int mNumber = 0;

    private float pReloadProgress { get { return Mathf.Clamp(Time.time - mTimeOfLastShot, 0f, ReloadDuration); } }
    private Color pColour { get { return (Faction == Faction.Player) ? PlayerColour : EnemyColour; } }


    //-----------------------------Unity Functions-----------------------------

    private void Start()
    {
        mHealth = MaxHealth;
        mTimeOfLastShot = Time.time - ReloadDuration;
        SetDestination(transform.position);

        foreach (var tintedObj in TintedObjects)
            tintedObj.material.color = pColour;

        if(Faction == Faction.Enemy)
            gameObject.AddComponent<EnemyAI>();

        mHealthBar = (Instantiate(HealthBarPrefab) as GameObject).GetComponent<UI_NodeBar>();
        mHealthBar.transform.SetParent(FindObjectOfType<Canvas>().transform);
        mHealthBar.transform.SetAsFirstSibling();
        mReloadBar = (Instantiate(ReloadBarPrefab) as GameObject).GetComponent<UI_Bar>();
        mReloadBar.transform.SetParent(FindObjectOfType<Canvas>().transform);
        mReloadBar.transform.SetAsFirstSibling();
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

    public void TakeDamage(int damage, Tank source)
    {
        mHealth -= damage;
        if (mHealth <= 0)
        {
            Instantiate(Resources.Load("Particles/Res_LargeExplosionParticles"), transform.position, Quaternion.identity);

            var obj = Instantiate(Resources.Load("Misc/Res_ExplosionDecal"), transform.position, Quaternion.identity) as GameObject;
            var pos = obj.transform.position;
            pos.y = 0.01f + Random.Range(0f, 0.01f);
            pos.x += Random.Range(-0.5f, 0.5f);
            pos.z += Random.Range(-0.5f, 0.5f);
            obj.transform.position = pos;
            var rot = obj.transform.eulerAngles;
            rot.y = Random.Range(0f, 360f);
            obj.transform.eulerAngles = rot;
            float scaleValue = Random.Range(0.3f, 0.6f);
            obj.transform.localScale = new Vector3(scaleValue, 1f, scaleValue);
            float colourValue = Random.Range(0f, 0.05f);
            obj.GetComponent<MeshRenderer>().material.color = new Color(colourValue, colourValue, colourValue, 0.6f);

            obj = Instantiate(Resources.Load("Misc/Res_AudioSource")) as GameObject;
            obj.GetComponent<AudioSourceManager>().PlayExplosion();

            DestroyImmediate(this.gameObject);
        }
        mTimeOfLastTakeDamage = Time.time;
        mTankThatLastDealtDamage = source;
    }

    public void SetTankNumber(int number)
    {
        mNumber = number;
        foreach(var mesh in TankNumberMeshes)
            mesh.text = "0" + number.ToString();
    }

    public bool IsBeingEngaged(out Tank source)
    {
        source = mTankThatLastDealtDamage;
        return (Time.time - mTimeOfLastTakeDamage < 4f);
    }


    //---------------------------Protected Functions---------------------------

    protected void Fire()
    {
        if (pReloadProgress < ReloadDuration) return;
        FireImmediate();
        mTimeOfLastShot = Time.time;
    }

    protected bool TargetVisibleFromTurret()
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


    //----------------------------Private Functions----------------------------

    private void UpdateUI()
    {
        mHealthBar.SetWorldPos(transform.position);
        mHealthBar.SetValue(mHealth, MaxHealth, pColour);
        mReloadBar.SetWorldPos(transform.position);
        mReloadBar.SetValue(pReloadProgress, ReloadDuration, pColour);
    }

    private void FireImmediate()
    {
        Vector3 projectileEndPos = ProjectileRaySource.position + TurretTrans.forward * Range;

        RaycastHit hit;
        if (Physics.Raycast(ProjectileRaySource.position, TurretTrans.forward, out hit, Range))
        {
            projectileEndPos = hit.point;
            if (hit.collider.GetComponent<Tank>())
            {
                var tank = hit.collider.GetComponent<Tank>();
                if (tank.Faction != Faction)
                {
                    tank.TakeDamage(DamagePerShot, this);
                    Instantiate(Resources.Load("Particles/Res_ExplosionParticles"), hit.point, Quaternion.identity);
                }
            }
        }

        var obj = Instantiate(Resources.Load("Misc/Res_ProjectileTrail")) as GameObject;
        obj.GetComponent<ProjectileTrail>().Init(ProjectileRaySource.position, projectileEndPos);

        Instantiate(Resources.Load("Particles/Res_ShootParticles"), ProjectileRaySource.position, TurretTrans.rotation);

        obj = Instantiate(Resources.Load("Misc/Res_AudioSource")) as GameObject;
        obj.GetComponent<AudioSourceManager>().PlayShot();
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
}