using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeliverPoint : MonoBehaviour
{
    public UnityEvent pickedUpR;
    public UnityEvent pickedUpNR;

    bool isActive=true;
    bool isreal =true;
    float cooldown;

    Collider2D boxColl;
    SpriteRenderer boxSprite;


    private void Start() {
        boxColl=GetComponent<Collider2D>();
        boxSprite=GetComponent<SpriteRenderer>();
        cooldown=Random.Range(20,41);
        if(Random.Range(1,101)<=40){//60% iniciate visible 40% not
            IsReal();
        }
        else{
            _SetActive(false);
        }
        IsReal();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Player"){
            if(isreal)
                pickedUpR.Invoke();//add secs to counter and score(REAL)
            else
                pickedUpNR.Invoke();//iniciate animation(NOT REAL)
            _SetActive(false);
        }
    }

    private void Update() {
        if(isActive==false){
            if(cooldown<=0){
                cooldown=Random.Range(1,15);
                _SetActive(true);
                IsReal();
            }
            cooldown-=Time.deltaTime;
        }
    }
    void _SetActive(bool active){
        boxColl.enabled= active;
        boxSprite.enabled= active;
        isActive=active;
    }
    void IsReal(){//handles whenever is real or not 20% isnot real 80% is real
        /*if(Random.Range(1,101)<=20) isreal=false;
        else isreal=true;*/
    }
}
