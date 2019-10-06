using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    protected ConnectedWaypoint _currentWaypoint;
    protected bool canSeePlayer = false;
    public bool chasingPlayer = false;
    protected Vector3 chaseLocation;
    protected float timeChasing = 30f;
    protected float timeToChase = 10f;
    protected float startingRotationY = 0f;
    protected float searchRotationY = 0f;
    protected bool searchReverse = false;
    Vector3 pointA;
    Vector3 pointB;
    protected bool rotationInit = false;
    protected float rotationSpeed = 0.46f;
    protected bool _travelling;

    protected NavMeshAgent _navMeshAgent;
    protected float walkSpeed = 3.5f;
    protected float walkAcceleration = 8f;
    protected float runSpeed = 5f;
    protected float runAcceleration = 10f;

    //Dictates whether the agent waits on each node.
    [SerializeField]
    protected bool _patrolWaiting = false;

    //The total time we wait at each node.
    [SerializeField]
    protected float _totalWaitTime = 3f;

    //The probability of switching direction.
    [SerializeField]
    protected float _switchProbability = 0.2f;

    public virtual void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        //PlayerDetection playerDetection = this.GetComponent<PlayerDetection>();
        //if (playerDetection == null)
        //{
        //  Debug.Log("player detec is null");
        //}
        //playerDetection.sawPlayerStealing.AddListener(OnSeePlayer);
        //this.GetComponent<PlayerDetection>().sawPlayerStealing.AddListener(OnSeePlayer);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        canSeePlayer = false;
        //loop through all rays and if collide with player canSeePlayer = true;

        if (chasingPlayer)
        {
            timeChasing -= Time.deltaTime;
            if (timeChasing <= 0)
            {
                //clear chasing parameters
                chasingPlayer = false;
                _navMeshAgent.acceleration = walkAcceleration;
                _navMeshAgent.speed = walkSpeed;
                _navMeshAgent.SetDestination(_currentWaypoint.transform.position);
                rotationInit = false;
            }
        }

        if (chasingPlayer && _navMeshAgent.remainingDistance == 0f)
        {
            //reached last known player location, look around for player
            //lessen time looking around if there is still a lot of time left
            if (timeChasing > 2.0f)
            {
                timeChasing = 2.0f;
            }

            //rotate body around so raycasts can scan for player
            if (!rotationInit)
            {
                //Get current position then add 90 to its Y axis
                pointA = transform.eulerAngles + new Vector3(0f, 120f, 0f);
                //Get current position then substract -90 to its Y axis
                pointB = transform.eulerAngles + new Vector3(0f, -120f, 0f);
                rotationInit = true;
            }
            float time = Mathf.PingPong(Time.time * rotationSpeed, 1);
            transform.eulerAngles = Vector3.Lerp(pointA, pointB, time);
        }
    }

    public void OnSeePlayer()
    {
        chaseLocation = GameObject.FindGameObjectWithTag("Player").transform.position;
        chasingPlayer = true;
        timeChasing = timeToChase;
        _navMeshAgent.acceleration = runAcceleration;
        _navMeshAgent.speed = runSpeed;
        ChasePlayer();
    }

    public virtual void ChasePlayer()
    {
        _navMeshAgent.SetDestination(chaseLocation);
        _travelling = true;
    }
}
