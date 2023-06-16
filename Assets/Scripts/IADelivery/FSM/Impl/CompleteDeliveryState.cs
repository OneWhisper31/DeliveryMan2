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


        public override void UpdateLoop()
        {
            //sumar un punto y replantear estrategia
            Debug.Log("God,No?");
        }

        public override IState ProcessInput()
        {
            return this;
        }
    }
}
