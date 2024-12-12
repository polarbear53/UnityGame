using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopSound : MonoBehaviour
{
    public AudioClip pop;
    public AudioClip levelup;
    public AudioClip hurt;
    public AudioSource audio;
    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }
}
