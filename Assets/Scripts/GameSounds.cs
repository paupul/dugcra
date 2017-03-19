using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSounds : MonoBehaviour {

    public AudioClip[] audioClips;

    public void PlaySound(int sound)
    {
        GetComponent<AudioSource>().clip = audioClips[sound];
        GetComponent<AudioSource>().Play();
    }
}
