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
    public Transform          pivotCar;
    public Rigidbody2D        rb;
    public List<DeliverPoint> delivers = new List<DeliverPoint>();
    public LayerMask          wallLayer;

    [Tooltip("CarRadius")]
    public float radius;

    CircleQuery circleQuery;

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
        rb = GetComponent<Rigidbody2D>();
        circleQuery = GetComponent<CircleQuery>();

        StartCoroutine(PlanAndExc());
    }
    IEnumerator PlanAndExc() { yield return new WaitForSecondsRealtime(1f); PlanAndExecute(); }
    public  void PlanAndExecute()
    {
        //IA II - P1 PUEDE SERVIR PARA SALIR
        /*var avaliablePoints = delivers.Where(x => x.isActive)
                                      .Select(x=>Tuple.Create(x.transform,false))//False=Delivers || True=Player
                                      .Concat(new Tuple<Transform, bool>[] { Tuple.Create(player.transform, true) })
                                      .OrderBy(x => (transform.position - x.Item1.position).magnitude);*/
        //IA II - P1/P2
        var playerOnRangeAndSight = circleQuery.Query()
                       .Select(x => (CarMovement)x)
                       .Where(x => x != null)
                       .Where(x => pivotCar.position.CanPassThrough(x.transform.position, radius, wallLayer));

        var deliveryIsActive = delivers.Where(x => x.isActive == true).OrderBy(x => (pivotCar.position - x.transform.position).magnitude);
        var deliveryLineOfSight = deliveryIsActive.Where(x => pivotCar.position.CanPassThrough(x.transform.position, radius, wallLayer));

        var actions = new List<GOAPAction>{
                                              new GOAPAction("Idle")
                                                 .Effect("hasPath", true)/*Could be seek, Steal or aStar*/
                                                 .LinkedState(idleState),

                                              new GOAPAction("Steal")
                                                 .Pre("hasPath", true)
                                                 .Pre("isPlayerInFOV",   true)
                                                 .Effect("objectiveAchieved", true)
                                                 .Cost(//IA - P1 
                                                  playerOnRangeAndSight.Any(x=>x==null)?1:
                                                  playerOnRangeAndSight.Take(1)
                                                  .Aggregate(1f,(x,y)=>(pivotCar.position - y.transform.position).magnitude)
                                                  )
                                                 .LinkedState(stealState),

                                              new GOAPAction("Seek")
                                                 .Pre("hasPath", true)
                                                 .Pre("isPlayerInFOV", false)
                                                 .Pre("isObjectiveInSight", true)
                                                 .Effect("objectiveAchieved",    true)
                                                 .Cost(//IA - P1
                                                  deliveryLineOfSight.Take(1)
                                                  .Aggregate(1f,(x,y)=>(pivotCar.position - y.transform.position).magnitude)
                                                  )
                                                 .LinkedState(seekState),

                                              new GOAPAction("AStar")
                                                 .Pre("hasPath", true)
                                                 .Pre("isPlayerInFOV", false)
                                                 .Pre("isObjectiveInSight",   false)
                                                 .Effect("objectiveAchieved", true)
                                                 .Cost(//IA - P1
                                                  deliveryIsActive.Take(1)
                                                  .Aggregate(1f,(x,y)=>(pivotCar.position - y.transform.position).magnitude)
                                                  )
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

        //StealState    || IA2 - P2
        from.values["isPlayerInFOV"] = playerOnRangeAndSight.Count() > 0;

        //AStar || Seek    || IA2 - P1
        from.values["isObjectiveInSight"] = deliveryLineOfSight.Count()>0;
        foreach (var item in deliveryLineOfSight)
        {
            Debug.Log(item.name);
        }

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
        float minSpeedAllowTurning = (rb.velocity.magnitude / 8);
        minSpeedAllowTurning = Mathf.Clamp01(minSpeedAllowTurning);

        //Update the rotation angle based on input
        rotationAngle -= steeringInput * turnFactor * minSpeedAllowTurning;

        rb.MoveRotation(rotationAngle);
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);

        rb.velocity = forwardVelocity + rightVelocity * driftFactor;
        //if player press space, the car drifts more
    }

    public void SetInputVector(Vector2 input, Vector3 target)
    {
        //steeringInput = (float)Math.Round(Vector2.Angle(input, transform.up) * 0.02f, 3, MidpointRounding.ToEven);
        steeringInput = Mathf.Lerp(steeringInput, Mathf.Clamp(Vector2.SignedAngle(input, pivotCar.up), -1, 1),0.3f);
        accelerationInput = Mathf.Max(Mathf.Abs(input.y * 2), 0.3f);

        if (Physics2D.Raycast(pivotCar.position, pivotCar.up,2f,wallLayer))
        {
            
            accelerationInput = -0.2f;
        }

    }
    public void SetBrakeVector()
    {
        //steeringInput = 0;
        accelerationInput = 0;
    }

    #endregion



    #region DrawGizmos
    /*
        private void OnDrawGizmos()
        {//first pregunte si any
            Gizmos.color = Color.red;
            var deliveryIsActive = delivers.Where(x => x.isActive == true).OrderBy(x => (transform.position - x.transform.position).magnitude);
            var deliveryLineOfSight = deliveryIsActive.Where(x => transform.position.CanPassThrough(x.transform.position, radius, wallLayer));

            foreach (var item in deliveryIsActive)
            {
                Gizmos.DrawLine(transform.position, item.transform.position);
                Gizmos.DrawLine(transform.position + Vector3.right * radius, item.transform.position + Vector3.right * radius);
                Gizmos.DrawLine(transform.position - Vector3.right * radius, item.transform.position - Vector3.right * radius);
                Gizmos.DrawLine(transform.position + Vector3.up * radius, item.transform.position + Vector3.up * radius);
                Gizmos.DrawLine(transform.position - Vector3.up * radius, item.transform.position - Vector3.up * radius);
            }
            Gizmos.color = Color.green;
            foreach (var item in deliveryLineOfSight)
            {
                Gizmos.DrawLine(transform.position, item.transform.position);
            }
        }*/
    #endregion
}



