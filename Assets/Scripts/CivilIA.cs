using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Civil{
public class CivilIA : MonoBehaviour
{
    
    public GameObject bloodParticle;
    public UICounter counter;
    

    float speed;

    Rigidbody2D rb;
    
    [HideInInspector]public Vector2 location;
    [HideInInspector] public bool isWaiting;

    private void Start() {
        StartCoroutine(WaitXSeconds());
        rb=GetComponent<Rigidbody2D>();
    }

    private void Update() {//if it is to close, search new location
        if(Vector2.Distance(transform.position,location)<=1&&!isWaiting){
            StartCoroutine(WaitXSeconds());
            
        }
    }

    private void FixedUpdate() {
        if(!isWaiting){//if isnt in cooldown, move towards
            transform.position = Vector2.MoveTowards(transform.position, location, speed*Time.fixedDeltaTime);
        }
    }

    void ChooseNewLocation(){//random coords
        location =Random.insideUnitCircle*Random.Range(20,151);
    }
    IEnumerator WaitXSeconds(){//wait seconds and restart functions
        isWaiting=true;

        yield return new WaitForSeconds(Random.Range(1f,2f));

        speed=Random.Range(10,21);
        isWaiting=false;
        ChooseNewLocation();
        
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Player"){
            Instantiate<GameObject>(bloodParticle,transform.position,transform.rotation);
            counter.SubstractSecs(5);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        //if collides with a wall, search new location
        if(other.tag=="Wall"&&!isWaiting){
            StartCoroutine(WaitXSeconds());
        }
    }

}}
