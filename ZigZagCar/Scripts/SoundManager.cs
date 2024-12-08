using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager sm;
    public AudioSource audiosource;
    public AudioClip Starsound, Diamondsound, Gameoversound;

    // Start is called before the first frame update
    private void Awake()
    {
        if (sm == null)
        {
            sm = this;
        }
    }
    public void starSound()
    {
        audiosource.clip = Starsound;
        audiosource.Play();
    }

    public void DiamondSound()
    {
        audiosource.clip = Diamondsound;
        audiosource.Play();
    }
    public void GameoverSound()
    {
        audiosource.clip = Gameoversound;
        audiosource.Play();
    }
}   