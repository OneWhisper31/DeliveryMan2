using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{

    public class CompleteDeliveryState : MonoBaseState
    {
        CarIA carIA;

        private void Awake()
        {
            carIA = GetComponent<CarIA>();
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            base.Enter(from, transitionParameters);
            carIA.PlanAndExecute();
        }

        public override void UpdateLoop()
        {
            
        }

        public override IState ProcessInput()
        {
            return this;
        }
    }
}
