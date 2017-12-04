using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceManager : MonoBehaviour {

    [SerializeField]
    private List<AudioClip> Shots = new List<AudioClip>();
    [SerializeField]
    private List<AudioClip> Explosions = new List<AudioClip>();
    [SerializeField]
    private List<AudioClip> RadioMessages = new List<AudioClip>();
    [SerializeField]
    private List<AudioClip> Equips = new List<AudioClip>();


    //----------------------------Public Functions-----------------------------

    public void PlayShot()
    {
        Play(Shots[Random.Range(0, Shots.Count)]);
    }

    public void PlayExplosion()
    {
        Play(Explosions[Random.Range(0, Explosions.Count)]);
    }

    public void PlayRadio()
    {
        Play(RadioMessages[Random.Range(0, RadioMessages.Count)]);
    }

    public void PlayEquip()
    {
        Play(Equips[Random.Range(0, Equips.Count)]);
    }


    //----------------------------Private Functions----------------------------

    private void Play(AudioClip clip)
    {
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }
}
