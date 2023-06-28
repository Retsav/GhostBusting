using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class AudioClipRefsSO : ScriptableObject
{
    public AudioClip[] qteSuccess;
    public AudioClip[] qteFail;
    public AudioClip[] footsteps;
    public AudioClip[] objectDrop;
    public AudioClip[] objectPickup;
    public AudioClip[] evilLaugh;
    public AudioClip[] particleExplosion;
    public AudioClip[] whispersAmbient;
    public AudioClip[] buttonClicks;
}
