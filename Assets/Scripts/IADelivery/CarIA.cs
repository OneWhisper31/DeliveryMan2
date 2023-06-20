using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FSM;
using System;

//IA2 - TP3
public class CarIA : MonoBehaviour
{

    #region Variables
    [Header("States")] //IA2-P3
    [SerializeField] IdleState             idleState;
    [SerializeField] SeekState             seekState;
    [SerializeField] AStarState            AStarState;
    [SerializeField] CompleteDeliveryState completeDeliveryState;
    [SerializeField] StealState            stealState;


    [Header("Variables GOAP y FSM")]
    public float viewRadiusPlayer;
    public float viewAnglePlayer;

    public Transform          player;
    public Rigidbody2D        myRb;
    public List<DeliverPoint> delivers = new List<DeliverPoint>();
    public LayerMask          wallLayer;

    [Header("Variables Movimiento de la Moto")]
    public float driftFactor = 0.95f;
    public float accelerationFactor = 30;
    public float turnFactor = 3.5f;
    public float maxSpeed = 20;

    float accelerationInput;
    float steeringInput;

    float rotationAngle;
    float velocityVsUp;


    //IA2-P3
    FiniteStateMachine _fsm;
    #endregion




    #region GOAP

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        PlanAndExecute();
    }
    public  void PlanAndExecute()
    {
        var actions = new List<GOAPAction>{
                                              new GOAPAction("Idle")
                                                 .Effect("hasPath", true)/*Could be seek, Steal or aStar*/
                                                 .LinkedState(idleState),

                                              new GOAPAction("Steal")
                                                 .Pre("hasPath", true)
                                                 .Pre("isPlayerInFOV",   true)
                                                 .Effect("objectiveAchieved", true)
                                                 .LinkedState(stealState),

                                              new GOAPAction("Seek")
                                                 .Pre("hasPath", true)
                                                 .Pre("isObjectiveInSight", true)
                                                 .Pre("isPlayerInFOV",   false)
                                                 .Effect("objectiveAchieved",    true)
                                                 .Cost(2)
                                                 .LinkedState(seekState),

                                              new GOAPAction("AStar")
                                                 .Pre("hasPath", true)
                                                 .Pre("isObjectiveInSight",   false)
                                                 .Pre("isPlayerInFOV",   false)
                                                 .Effect("objectiveAchieved", true)
                                                 .Cost(2)
                                                 .LinkedState(AStarState),

                                              new GOAPAction("CompleteDelivery")
                                                 .Pre("objectiveAchieved",   true)
                                                 .Effect("complete", true)
                                                 .LinkedState(completeDeliveryState)
                                          };

        var from = new GOAPState();

        //defaultsValues
        from.values["hasPath"] =            false;
        from.values["objectiveAchieved"] = false;
        from.values["complete"] = false;

        //StealState
        from.values["isPlayerInFOV"] = 
            Physics2DExtension.InFieldOfView(transform.position,player.position,viewRadiusPlayer, viewAnglePlayer, wallLayer);

        //AStar || Seek
        from.values["isObjectiveInSight"] = 
            //IA2 - TP1
            delivers.Any(x => x.isActive == true && Physics2DExtension.InLineOfSight(transform.position, x.transform.position, wallLayer));


                    /*.Any(x => x != null);*/

                    /*.OrderBy(x =>(transform.position- x.transform.position).magnitude) Para el state
                    .First();*/

        

        var to = new GOAPState();
        to.values["complete"] =             true;

        var planner = new GoapPlanner();
        planner.OnPlanCompleted += OnPlanCompleted;
        planner.OnCantPlan += OnCantPlan;

        planner.Run(from, to, actions, StartCoroutine);
    }


    private void OnPlanCompleted(IEnumerable<GOAPAction> plan)
    {
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }

    private void OnCantPlan()
    {
        Debug.LogError("[Custom Error] - CAN'T PLAN");
    }

    #endregion




    #region MovementLogic



    private void FixedUpdate()
    {
        ApplyEngineForce();
        //to make the car drift
        KillOrthogonalVelocity();
        ApplySteering();
    }
    void ApplyEngineForce()
    {
        var rb = myRb;

        velocityVsUp = Vector2.Dot(transform.up, rb.velocity);

        if (velocityVsUp > maxSpeed && accelerationInput > 0)
            return;
        if (velocityVsUp < maxSpeed * -0.5f && accelerationInput < 0)
            return;
        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
            return;


        if (accelerationInput == 0)
            rb.drag = Mathf.Lerp(rb.drag, 3, Time.fixedDeltaTime * 5);
        else rb.drag = 0;

        Vector2 forceVector = transform.up * accelerationInput * accelerationFactor;

        rb.AddForce(forceVector, ForceMode2D.Force);
    }
    void ApplySteering()
    {

        //Limit the car ability to turn when moving slowly
        float minSpeedAllowTurning = (myRb.velocity.magnitude / 8);
        minSpeedAllowTurning = Mathf.Clamp01(minSpeedAllowTurning);

        //Update the rotation angle based on input
        rotationAngle -= steeringInput * turnFactor * minSpeedAllowTurning;

        myRb.MoveRotation(rotationAngle);
    }

    void KillOrthogonalVelocity()
    {
        var rb = myRb;

        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);

        rb.velocity = forwardVelocity+ rightVelocity * driftFactor;
        //if player press space, the car drifts more
    }

    public void SetInputVector(Vector2 input)
    {
        float angle = Vector3.Angle(input, transform.up);

        if (Mathf.Abs(angle) <= 20)
            steeringInput = 0;
        else
            steeringInput = angle * 0.02f;

        steeringInput = (float)Math.Round(steeringInput, 3,MidpointRounding.ToEven);

        accelerationInput = Mathf.Max(Mathf.Abs(input.y*2),0.2f);
            
    }

    #endregion
}

