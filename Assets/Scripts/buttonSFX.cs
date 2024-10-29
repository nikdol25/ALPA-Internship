using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonSFX : MonoBehaviour
{
    public AudioSource myFx;
    public AudioClip clickFx;

    public void ClickSound()
    {
        myFx.PlayOneShot(clickFx);
    }
}