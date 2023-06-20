using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeliverPoint : MonoBehaviour
{

    //bool isreal = true;
    float cooldown;

    Collider2D boxColl;
    SpriteRenderer boxSprite;


    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _audioclip;

    bool _isActive;
    public bool isActive {
        get
        {
            return _isActive;
        }
        private set {
            boxColl.enabled = value;
            boxSprite.enabled = value;
            _isActive= value;
        }
    }


    private void Start() {
        boxColl=GetComponent<Collider2D>();
        boxSprite=GetComponent<SpriteRenderer>();
        cooldown=Random.Range(20,41);
        isActive = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Player1"){
            _audio.PlayOneShot(_audioclip);
            UICounter.intance.AddScore(Player.One);
            isActive =false;
        }
        else if (other.tag == "Player2")
        {
            _audio.PlayOneShot(_audioclip);
            UICounter.intance.AddScore(Player.Two);
            isActive = false;
        }
    }

    private void Update() {
        if(isActive==false){
            if(cooldown<=0){
                cooldown=Random.Range(1,15);
                isActive = true;
                //IsReal();
            }
            cooldown-=Time.deltaTime;
        }
    }
}
