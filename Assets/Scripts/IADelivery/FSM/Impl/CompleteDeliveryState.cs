using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{

    public class CompleteDeliveryState : MonoBaseState
    {
        [SerializeField] float cooldownWait = 3;

        CarIA carIA;

        bool changeState;

        private void Awake()
        {
            carIA = GetComponent<CarIA>();
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            base.Enter(from, transitionParameters);
            StartCoroutine(PlanAndExecute());
        }
        public override void UpdateLoop()
        {
            carIA.SetBrakeVector();
        }
        public override IState ProcessInput()
        {
            if (changeState)
            {
                _fsm.Active = false;
                carIA.PlanAndExecute();
                changeState = false;
            }
            return this;
        }
        IEnumerator PlanAndExecute() {
            float time= Time.time+cooldownWait;
            while (time>Time.time)
            {
                carIA.SetBrakeVector();
                yield return new WaitForEndOfFrame();
            }
            changeState = true;
        }
    }
}
