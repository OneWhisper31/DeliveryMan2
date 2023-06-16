using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class SeekState : MonoBaseState
    {
        CarIA carIA;

        private void Awake()
        {
            carIA = GetComponent<CarIA>();
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