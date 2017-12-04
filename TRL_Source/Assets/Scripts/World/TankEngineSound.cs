using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TankEngineSound : MonoBehaviour {

    [SerializeField]
    private float MaxVolume = 1f;

    private AudioSource mSource;

	
	void Start ()
    {
        mSource = GetComponent<AudioSource>();
        StartCoroutine(UpdateSound());
	}
	
	
	IEnumerator UpdateSound()
    {
        while (true)
        {
            if (Time.timeScale > 0.1f)
            {
                bool play = false;
                foreach (var tank in FindObjectsOfType<Tank>())
                {
                    if (tank.pIsMoving)
                    {
                        play = true;
                        break;
                    }
                }
                mSource.volume = (play) ? MaxVolume : 0f;
            }

            yield return new WaitForSeconds(0.05f);
        }
	}

    public void Mute()
    {
        mSource.volume = 0f;
    }
}
