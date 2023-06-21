using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarMovement : MonoBehaviour, IGridEntity
{
    public float driftFactor=0.95f;
    public float accelerationFactor = 30;
    public float turnFactor =3.5f;
    public float maxSpeed = 20;

    float accelerationInput;
    float steeringInput;

    float rotationAngle;
    float velocityVsUp;

    Rigidbody2D rb;

    //SpatialGrid
    public event Action<IGridEntity> OnMove;
    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        ApplyEngineForce();
        //to make the car drift
        KillOrthogonalVelocity();
        ApplySteering();
    }
    void ApplyEngineForce(){

        velocityVsUp = Vector2.Dot(transform.up, rb.velocity);

        if(velocityVsUp>maxSpeed&&accelerationInput>0)
            return;
        if(velocityVsUp<maxSpeed*-0.5f&&accelerationInput<0)
            return;
        if(rb.velocity.sqrMagnitude>maxSpeed*maxSpeed&&accelerationInput>0)
            return;

        
        if(accelerationInput==0)
            rb.drag=Mathf.Lerp(rb.drag,3,Time.fixedDeltaTime*5);
        else rb.drag=0;

        Vector2 forceVector = transform.up*accelerationInput*accelerationFactor; 

        rb.AddForce(forceVector, ForceMode2D.Force);
    }
    void ApplySteering(){

        //Limit the car ability to turn when moving slowly
        float minSpeedAllowTurning=(rb.velocity.magnitude/8);
        minSpeedAllowTurning=Mathf.Clamp01(minSpeedAllowTurning);

        //Update the rotation angle based on input
        rotationAngle -=steeringInput*turnFactor*minSpeedAllowTurning;

        rb.MoveRotation(rotationAngle);
    }

    void KillOrthogonalVelocity(){
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity,transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity,transform.right);

        rb.velocity=forwardVelocity+rightVelocity*driftFactor;
        //if player press space, the car drifts more
    }
    public void SetInputVector(Vector2 input){
        steeringInput=input.x;
        accelerationInput=input.y;
    }
}
