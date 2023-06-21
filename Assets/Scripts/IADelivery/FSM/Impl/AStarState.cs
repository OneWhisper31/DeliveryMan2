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

            if (carIA.delivers.Any(x => x.isActive))
            {
                //IA2-P1
                DeliverPoint deliver = 
                carIA.delivers.Where(x => x.isActive)
                              .OrderBy(x => (transform.position - x.transform.position).magnitude)
                              .First();

                Debug.Log(deliver.name);

                _path = TrackNewPath(transform.position, deliver.transform.position);

                foreach (var item in _path)
                {
                    Debug.Log(item.name+" "+ item.locked);
                    
                }
                dir = TrackNewTarget();

                
            }

        }

        public override void UpdateLoop()
        {
            
            if (_target != null)
                dir = _target - (Vector2)transform.position;

            if (_path.Count > 0)
            {
                if (dir.magnitude < 3)
                {
                    dir = TrackNewTarget();
                }
            }

            carIA.SetInputVector(dir.normalized);
        }

        public override IState ProcessInput()
        {
            if (_path.Count <= 0 && dir.magnitude < 3&& Transitions.ContainsKey("OnCompleteDeliveryState"))
                return Transitions["OnCompleteDeliveryState"];
            
            return this;
        }

        public List<Node> TrackNewPath(Vector3 start, Vector3 end)
        {
            Node startNode = nodeGrid.GetStartingNode(start);
            Node endNode = nodeGrid.GetStartingNode(end);

            return _pathfinding.ThetaStar(startNode, endNode,
                Vector3.Distance(startNode.transform.position, transform.position) < 3f ?
                    true : false/*si esta cerca del nodo inicial lo evita*/);
        }
        public Vector3 TrackNewTarget()
        {
            if (_path.Count <= 0)
                return default;

            _target = _path[0].transform.position;
            _path.RemoveAt(0);

            return _target - (Vector2)transform.position;
        }
    }
}