﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySimplePatrol : BaseEnemy
{
    //The list of all patrol nodes to visit.
    [SerializeField]
    List<Waypoint> _patrolPoints;

    //Private variables for base behaviour.
    int _currentPatrolIndex;
    bool _waiting;
    bool _patrolForward;
    float _waitTimer;

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
            if (_patrolPoints != null && _patrolPoints.Count >= 2)
            {
                _currentPatrolIndex = 0;
                SetDestination();
            }
            else
            {
                Debug.Log("Insufficient patrol points for basic patrolling behaviour.");
            }
        }
        //TODO: change this to sawPlayerStealing
        this.GetComponent<PlayerDetection>().sawPlayer.AddListener(OnSeePlayer);
    }
    public override void Update()
    {
        base.Update();
        //Check if we're close to the destination.
        if (_travelling && _navMeshAgent.remainingDistance <= 1.0f)
        {
            _travelling = false;

            //If we're going to wait, then wait.
            if (_patrolWaiting)
            {
                _waiting = true;
                _waitTimer = 0f;
            }
            else
            {
                ChangePatrolPoint();
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

                ChangePatrolPoint();
                SetDestination();
            }
        }
    }

    private void SetDestination()
    {
        if (chasingPlayer)
        {
            base.ChasePlayer();
        }
        else
        {
            if (_patrolPoints != null)
            {
                Vector3 targetVector = _patrolPoints[_currentPatrolIndex].transform.position;
                _currentWaypoint = (ConnectedWaypoint)_patrolPoints[_currentPatrolIndex];
                _navMeshAgent.SetDestination(targetVector);
            }
        }
        _travelling = true;
    }

    /// <summary>
    /// Selects a new patrol point in the available list, but
    /// also with a small probability allows for us to move forward or backwards.
    /// </summary>
    private void ChangePatrolPoint()
    {
        if (UnityEngine.Random.Range(0f, 1f) <= _switchProbability)
        {
            _patrolForward = !_patrolForward;
        }

        if (_patrolForward)
        {
            _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
        }
        else
        {
            if (--_currentPatrolIndex < 0)
            {
                _currentPatrolIndex = _patrolPoints.Count - 1;
            }
        }
    }
}