using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour {

    [SerializeField]
    private float LifeTime = 1f;


    //-----------------------------Unity Functions-----------------------------

    private void Awake()
    {
        StartCoroutine(SelfDestruct());
    }


    //----------------------------Private Functions----------------------------

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(gameObject);
    }
}
