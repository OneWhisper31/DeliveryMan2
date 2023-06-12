using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Civil{
public class CivilAnim : MonoBehaviour
{
    Animator anim;
    CivilIA ia;
    Rigidbody2D rb;

    float angleTowardsLocation;

    private void Start() {
        anim=GetComponent<Animator>();
        ia=GetComponent<CivilIA>();
        rb=GetComponent<Rigidbody2D>();
    }

    private void Update() {
        anim.SetBool("Idle",ia.isWaiting);
        Vector3 dir=new Vector3(ia.location.x,ia.location.y,0)-transform.position;
        angleTowardsLocation= Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg;
        GetComponent<Rigidbody2D>().rotation=Mathf.Lerp(rb.rotation,angleTowardsLocation,0.2f);
    }
}}