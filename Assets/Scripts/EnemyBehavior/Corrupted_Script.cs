using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Corrupted_Script : MonoBehaviour
{
    public bool isHoldingPlayer;

    [HideInInspector] public NavMeshAgent agent;
    public GameObject capturePoint;
    public GameObject playerSocket;

    [HideInInspector] public Transform player;
    [HideInInspector] public CrouchToHide_Script playerHide;

    public LayerMask whatIsGround, whatIsPlayer;

    //corrupted patrolling code

    public Vector3 walkPoint1;
    public Vector3 walkPoint2;
    bool walkPointSet1;
    bool walkPointSet2;
    public float walkPointRange;
    private bool roam = false;

    public Transform[] points;
    int current;
    public float speed;
    public float playerspeed;

    private float timer;

    public GameObject waypoint1;
    public GameObject waypoint2;
    public GameObject waypoint3;
    private Vector3 waypoint;

    //state of corrupted

    public float sightRange;
    private bool playerInSightRange;

    public float attackRange;
    private bool playerInAttackRange;

    private bool chasing;

    [SerializeField] private float grabBreakoutStun = 3;
    private float think;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (capturePoint == null)
        {
            Handles.Label(transform.position, "ERROR, no egg targeted for return");
        }

        if(agent.isStopped == true)
        {
            Handles.Label(transform.position + new Vector3(0, 6, 0), "Thinking...");
        }
    }
#endif

    void Start()
    {
        waypoint = waypoint1.transform.position;
        current = 0;
        //speed = 3;
    }

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        playerHide = GameObject.Find("Player").GetComponent<CrouchToHide_Script>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (think > 0)
        {
            think -= Time.deltaTime;

            if(think <= 0)
            {
                agent.isStopped = false;
            }
        }
        

        if (timer > 0) timer -= Time.deltaTime;

        if (isHoldingPlayer == false)
        {
            //Checking for sight range (and eventually atttack range)
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange) Patroling();

            //only chase player if in range, not hiding, and not stunned
            if (playerInSightRange && playerHide.hiding == false && timer <= 0) ChasePlayer();
            if (chasing && playerHide.hiding)
            {
                StopMoving();
            }

            //if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }

        //Override movement for when player is grabbed, and enemy look at destination
        if (isHoldingPlayer == true)
        {
            TakePlayerToEgg();
        }

    }

    private void Patroling()
    {
        if (!walkPointSet1)
        {
            if(roam)
            {
                Roam();
            }
            NewWaypoint();
        }

        if (walkPointSet1)
        {
            
            agent.SetDestination(walkPoint1);
        }

        if (walkPointSet2)
        {

            agent.SetDestination(walkPoint2);
        }


        Vector3 distanceToWalkPoint1 = transform.position - walkPoint1;
        Vector3 distanceToWalkPoint2 = transform.position - walkPoint2;

        //If main walkpoint is reached
        if (distanceToWalkPoint1.magnitude < 1f)
        {
            Think();
            roam = true;
            walkPointSet1 = false;
        }

        //If roam walkpoint is reached
        if (distanceToWalkPoint2.magnitude < 1f)
        {
            //Think();
            roam = false;
            walkPointSet2 = false;
        }

    }

    private void SearchWalkPoint(Vector3 waypoint)
    {
        walkPoint1 = waypoint;

        if (Physics.Raycast(walkPoint1, -transform.up, 2f, whatIsGround))
        {
            walkPointSet1 = true;
        }

    }

    //Temporary----------------------------------------------------
    private void Roam()
    {
        //Calculates random point in range

        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint2 = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint2, -transform.up, 2f, whatIsGround))
        {
            walkPointSet2 = true;
        }

    }

    private void NewWaypoint()
    {
        //swaps waypoints from the one that was just reached with another

        if (waypoint == waypoint1.transform.position)
        {
            waypoint = waypoint2.transform.position;
            SearchWalkPoint(waypoint);
        }


        else if (waypoint == waypoint2.transform.position)
        {
            waypoint = waypoint3.transform.position;
            SearchWalkPoint(waypoint);
        }


        else if (waypoint == waypoint3.transform.position)
        {
            waypoint = waypoint1.transform.position;
            SearchWalkPoint(waypoint);
        }

    }

    private void Think()
    {
        //Wait time before moving onto next waypoint
        think = Random.Range(1, 4);

        agent.isStopped = true;
    }
    //-------------------------------------------------------------

    private void TakePlayerToEgg()
    {
        //transform.LookAt(capturePoint.transform.position + transform.position);
        //transform.position = Vector3.MoveTowards(transform.position, capturePoint.transform.position - (playerSocket.transform.position - transform.position), speed * Time.deltaTime);

        agent.SetDestination(capturePoint.transform.position - (playerSocket.transform.position - transform.position));

        player.transform.position = Vector3.MoveTowards(player.transform.position, playerSocket.transform.position, playerspeed * Time.deltaTime);
    }

    private void ChasePlayer()
    {
        chasing = true;

        agent.speed = 10;

        agent.SetDestination(player.position);
        //transform.LookAt(player.position + transform.position);
    }

    private void StopMoving()
    {
        chasing = false;
        agent.SetDestination(gameObject.transform.position);
    }

    private void AttackPlayer()
    {
    }

    public void StunThisEnemy()
    {
        timer = grabBreakoutStun;
        isHoldingPlayer = false;

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LyreTrigger")
        {
            Debug.Log("Lyre Collider stunned corrupted"); //Being read but speed is not changing

            StunThisEnemy();
        }

        if (other.gameObject.tag == "Player" && timer <= 0)
        {
            Struggle struggleScript = other.gameObject.GetComponent<Struggle>();

            struggleScript.StartStruggling(this.gameObject.GetComponent<Corrupted_Script>());

            isHoldingPlayer = true;

            Debug.Log("Corrupted grabbed the player.");
        }

    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(walkPoint1, 1);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(walkPoint2, 1);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(waypoint1.transform.position, waypoint2.transform.position);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(waypoint2.transform.position, waypoint3.transform.position);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(waypoint3.transform.position, waypoint1.transform.position);
    }


    //when Corrupted is no longer colliding with lyre collider sphere
    /**void OnCollisionExit(Collision other)
    {
        speed = 3;
    }**/
}