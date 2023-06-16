using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{

    public class StealState : MonoBaseState
    {
        CarIA carIA;

        [SerializeField] Rigidbody2D playerRb;

        private void Awake()
        {
            carIA = GetComponent<CarIA>();
        }

        public override void UpdateLoop()
        {

            Vector2 futurePos = (Vector2)playerRb.transform.position + playerRb.velocity;
            Vector2 desired = futurePos - (Vector2)transform.position;

            carIA.SetInputVector(desired.normalized);
        }

        public override IState ProcessInput()
        {
            Vector2 futurePos = (Vector2)playerRb.transform.position + playerRb.velocity;

            if ((futurePos - (Vector2)transform.position).magnitude< 5 && Transitions.ContainsKey("OnCompleteDeliveryState"))
                return Transitions["OnCompleteDeliveryState"];

                return this;
        }
    }
}