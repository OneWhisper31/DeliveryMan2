using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace FSM
{

    public class StealState : MonoBaseState
    {
        CarIA carIA;

        [SerializeField] Rigidbody2D playerRb;
        [SerializeField] CircleQuery query;

        private void Awake()
        {
            carIA = GetComponent<CarIA>();
            query = GetComponent<CircleQuery>();
        }

        public override void UpdateLoop()
        {

            Vector2 futurePos = (Vector2)playerRb.transform.position + playerRb.velocity;
            Vector2 desired = futurePos - (Vector2)transform.position;

            carIA.SetInputVector(desired.normalized);
        }

        public override IState ProcessInput()
        {

            if ((playerRb.transform.position - transform.position).magnitude< 7 && Transitions.ContainsKey("OnCompleteDeliveryState"))
            {
                //IA2-P1/P2
                /*if(query.Query().Select(x => (CarMovement)x)
                             .Where(x => x != null)
                             .Where(x => x.tag == "Player1")
                             .Take(1)!=null)*/
                UICounter.intance.StealScore(Player.Two);

                return Transitions["OnCompleteDeliveryState"];
            }
            return this;
        }
    }
}