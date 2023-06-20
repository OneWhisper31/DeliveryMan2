using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace FSM
{

    public class AStarState : MonoBaseState
    {
        CarIA carIA;

        [SerializeField] NodeGrid nodeGrid;

        List<Node> _path = new List<Node>();
        Pathfinding _pathfinding = new Pathfinding();
        Vector2 _target;
        Vector2 dir;

        private void Awake()
        {
            carIA = GetComponent<CarIA>();
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            base.Enter(from, transitionParameters);

            if (carIA.delivers.Any(x => x.isActive == true))
            {
                _path = TrackNewPath(transform.position,
                //IA2-P1
                carIA.delivers.Where(x => x.isActive == true)
                .Aggregate(Tuple.Create(Vector3.zero, -1f), (x, y) =>
                {
                    float xMagnitude = (transform.position - x.Item1).magnitude;
                    float yMagnitude = (transform.position - y.transform.position).magnitude;

                    //Replace delivery for the closeone, if activated
                    if (y.isActive == true)
                        if (Physics2DExtension.InLineOfSight(transform.position, y.transform.position, carIA.wallLayer))
                            if ((xMagnitude > yMagnitude) || x.Item2 == -1/*FirstTime*/)
                                x = Tuple.Create(y.transform.position, yMagnitude);

                    return x;
                }).Item1);
                dir = TrackNewTarget();
            }

        }

        public override void UpdateLoop()
        {

            if (_target != null)
                dir = _target - (Vector2)transform.position;

            if (_path.Count > 0)
            {
                if (dir.magnitude < 0.3f)
                {
                    dir = TrackNewTarget();
                }
            }

            carIA.SetInputVector(dir.normalized);
        }

        public override IState ProcessInput()
        {
            if (_path.Count <= 0 && dir.magnitude < 0.3f&& Transitions.ContainsKey("OnCompleteDeliveryState"))
                return Transitions["OnCompleteDeliveryState"];
            else if (carIA.delivers.All(x => x.isActive == false) && Transitions.ContainsKey("OnIdleState"))
                return Transitions["OnIdleState"];
            
            return this;
        }

        public List<Node> TrackNewPath(Vector2 start, Vector2 end)
        {
            Node startNode = nodeGrid.GetStartingNode(start);

            return _pathfinding.ThetaStar(startNode, nodeGrid.GetStartingNode(end),
                Vector2.Distance(startNode.transform.position, transform.position) < 2f ?
                    true : false/*si esta cerca del nodo inicial lo evita*/);
        }
        public Vector2 TrackNewTarget()
        {
            if (_path.Count <= 0)
                return default;

            _target = _path[0].transform.position;
            _path.RemoveAt(0);

            return _target - (Vector2)transform.position;
        }
    }
}