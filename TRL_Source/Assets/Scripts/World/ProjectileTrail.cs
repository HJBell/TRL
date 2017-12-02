using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ProjectileTrail : MonoBehaviour {

    [SerializeField]
    private float LifeTime = 1f;

    private LineRenderer mLineRenderer;
    private float mSpawnTime = 0f;
    private float mInitAlpha = 0f;


    //-----------------------------Unity Functions-----------------------------

    private void Awake()
    {
        mLineRenderer = GetComponent<LineRenderer>();
        mSpawnTime = Time.time;
        mInitAlpha = mLineRenderer.startColor.a;
    }

    private void Update()
    {
        float timeElapsed = Time.time - mSpawnTime;

        if (timeElapsed >= LifeTime)
            Destroy(this.gameObject);

        float alpha = Mathf.Clamp01(1f - timeElapsed / LifeTime);

        var startCol = mLineRenderer.startColor;
        startCol.a = mInitAlpha * alpha;
        mLineRenderer.startColor = startCol;
    }


    //----------------------------Public Functions-----------------------------

    public void Init(Vector3 startPos, Vector3 endPos)
    {
        mLineRenderer.positionCount = 2;
        mLineRenderer.SetPosition(0, startPos);
        mLineRenderer.SetPosition(1, endPos);
    }
}
