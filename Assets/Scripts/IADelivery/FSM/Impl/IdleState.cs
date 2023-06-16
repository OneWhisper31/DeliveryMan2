using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace FSM
{

    public class IdleState : MonoBaseState
    {
        CarIA carIA;

        private void Awake()
        {
            carIA = GetComponent<CarIA>();
        }

        public override void UpdateLoop()
        {
            Debug.Log("idle");
        }
        public override IState ProcessInput()/*(carIA.player.transform.position-transform.position).magnitude*/
        {
            bool InFieldOfView = Physics2DExtension.InFieldOfView(transform.position, carIA.player.transform.position,
                                                 carIA.viewRadiusPlayer, carIA.viewAnglePlayer, carIA.wallLayer);

            //IA2-P1
            bool InLineOfSight = carIA.delivers.Any(x=>x.isActive==true)?
                Physics2DExtension.InLineOfSight(transform.position, 
                carIA.delivers.Where(x=>x.isActive==true)
                              .OrderBy(x => (transform.position - x.transform.position).magnitude)
                              .First().transform.position,carIA.wallLayer):false;

            if (Transitions.ContainsKey("OnStealState")&&InFieldOfView)                
            {
                return Transitions["OnStealState"];
            }
            else if (Transitions.ContainsKey("OnSeekState")&&InLineOfSight)
            {
                return Transitions["OnSeekState"];
            }
            else if (Transitions.ContainsKey("OnAStarState")&& !InLineOfSight)
            {
                return Transitions["OnAStarState"];
            }
            else
                return this;
        }
    }
}