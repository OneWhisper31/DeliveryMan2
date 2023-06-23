using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace FSM
{
    public class SeekState : MonoBaseState
    {
        CarIA carIA;
        Vector3 deliveryTarget;

        private void Awake()
        {
            carIA = GetComponent<CarIA>();
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            base.Enter(from, transitionParameters);


            //IA2-P1
            deliveryTarget = carIA.delivers
            .Where(x => x.isActive == true)
            .Aggregate(Tuple.Create(Vector3.zero, -1f), (x, y) =>
            {
                float xMagnitude = (transform.position - x.Item1).magnitude;
                float yMagnitude = (transform.position - y.transform.position).magnitude;

                //Replace delivery for the closeone, if activated
                if (y.isActive == true)
                    if (transform.position.CanPassThrough(y.transform.position,carIA.radius, carIA.wallLayer))
                        if ((xMagnitude > yMagnitude) || x.Item2 == -1/*FirstTime*/)
                        {

                            x = Tuple.Create(y.transform.position, yMagnitude);
                            Debug.Log(y.name);

                        }

                return x;
            }).Item1;
        }


        public override void UpdateLoop()
        {

            Vector2 desired = deliveryTarget - transform.position;

            carIA.SetInputVector(desired.normalized, deliveryTarget);
        }

        public override IState ProcessInput()
        {

            if ((deliveryTarget - transform.position).magnitude < 5
                && Transitions.ContainsKey("OnCompleteDeliveryState"))
                return Transitions["OnCompleteDeliveryState"];

            return this;
        }
    }
}