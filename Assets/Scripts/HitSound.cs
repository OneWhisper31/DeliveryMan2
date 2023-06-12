using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSound : MonoBehaviour{

    public AudioClip hitAudio;

    public AudioClip civilAudio;

    AudioSource audioSource;
    void Start(){
        audioSource=GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Wall"){
            audioSource.PlayOneShot(hitAudio);
            audioSource.volume=1;            
        }
        else if(other.tag=="Civil"){
            audioSource.PlayOneShot(civilAudio);
            audioSource.volume=0.5f;
        }
    }
}
