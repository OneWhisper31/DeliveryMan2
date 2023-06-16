using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeliverPoint : MonoBehaviour
{
    public UnityEvent pickedUpR;
    public UnityEvent pickedUpNR;

    bool isreal = true;
    float cooldown;

    Collider2D boxColl;
    SpriteRenderer boxSprite;

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
        isActive = false;
        /*if(Random.Range(1,101)<=40){//60% iniciate visible 40% not
            IsReal();
        }
        else{
            isActive = false;
        }
        IsReal();*/
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Player"){
            if(isreal)
                pickedUpR.Invoke();//add secs to counter and score(REAL)
            else
                pickedUpNR.Invoke();//iniciate animation(NOT REAL)
            isActive=false;
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
    /*void IsReal(){//handles whenever is real or not 20% isnot real 80% is real
        /*if(Random.Range(1,101)<=20) isreal=false;
        else isreal=true;
    }*/
    }
