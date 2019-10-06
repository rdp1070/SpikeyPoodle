using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Code
{
    public class EnemyConnectedPatrol : BaseEnemy
    {
        //Private variables for base behaviour.
        ConnectedWaypoint _previousWaypoint;

        bool _travelling;
        bool _waiting;
        float _waitTimer;
        int _waypointsVisited;

        // Use this for initialization
        public override void Start()
        {
            base.Start();

            if (_navMeshAgent == null)
            {
                Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
            }
            else
            {
                _navMeshAgent.acceleration = runAcceleration;
                _navMeshAgent.speed = runSpeed;

                if (_currentWaypoint == null)
                {
                    //Set it at random.
                    //Grab all waypoint objects in scene.
                    GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

                    if (allWaypoints.Length > 0)
                    {
                        while (_currentWaypoint == null)
                        {
                            int random = UnityEngine.Random.Range(0, allWaypoints.Length);
                            ConnectedWaypoint startingWaypoint = allWaypoints[random].GetComponent<ConnectedWaypoint>();

                            //i.e. we found a waypoint.
                            if (startingWaypoint != null)
                            {
                                _currentWaypoint = startingWaypoint;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("Failed to find any waypoints for use in the scene.");
                    }
                }

                SetDestination();
            }
        }

        public void Update()
        {
            base.Update();
            //Check if we're close to the destination.
            if (_travelling && _navMeshAgent.remainingDistance <= 1.0f)
            {
                _travelling = false;
                _waypointsVisited++;

                //If we're going to wait, then wait.
                if (_patrolWaiting)
                {
                    _waiting = true;
                    _waitTimer = 0f;
                }
                else
                {
                    SetDestination();
                }
            }

            //Instead if we're waiting.
            if (_waiting)
            {
                _waitTimer += Time.deltaTime;
                if (_waitTimer >= _totalWaitTime)
                {
                    _waiting = false;

                    SetDestination();
                }
            }
        }

        private void SetDestination()
        {
            if (chasingPlayer)
            {
                base.OnSeePlayer();
                base.ChasePlayer();
            }
            else
            {
                if (_waypointsVisited > 0)
                {
                    ConnectedWaypoint nextWaypoint = _currentWaypoint.NextWaypoint(_previousWaypoint);
                    _previousWaypoint = _currentWaypoint;
                    _currentWaypoint = nextWaypoint;
                }

                Vector3 targetVector = _currentWaypoint.transform.position;
                _navMeshAgent.SetDestination(targetVector);
                _travelling = true;
            }
        }
    }
}
