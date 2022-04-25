using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Corrupted_Script : MonoBehaviour
{
    #region Variables
    [Header("- General")]
    public float speed;
    public float playerspeed;
    public LayerMask whatIsGround, whatIsPlayer;
    private float thrust = 3f;
    public bool enemyRanIntoTable = false;

    [Header("- Scripts / Components")]
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Transform player;
    [HideInInspector] public CrouchToHide_Script playerHide;
    public C_Animation corrAnimationScript;


    [Header("- Pathing")]
    public GameObject waypoint1;
    public GameObject waypoint2;
    public GameObject waypoint3;
    private Vector3 waypoint;
    private bool walkPointSet1;
    private bool walkPointSet2;
    private float think;
    private bool thinking;
    private bool roam = false;
    [HideInInspector] public float walkPointRange;
    [HideInInspector] public Vector3 walkPoint1;
    [HideInInspector] public Vector3 walkPoint2;


    [Header("- Grabbing Player")]
    public GameObject capturePoint;
    public GameObject playerSocket;
    [HideInInspector] public bool isHoldingPlayer;
    [SerializeField] private float grabBreakoutStun = 3;
    private float stunTimer;

    [Header("- States")]
    public float sightRange;
    private bool playerInSightRange;

    private float attackRange;
    private bool playerInAttackRange;

    private bool chasing;
    #endregion

    #region Active Gizmos
    //Gizmos
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (capturePoint == null)
        {
            Handles.Label(transform.position, "ERROR, no egg targeted for return");
        }

        if (waypoint1 == null || waypoint2 == null || waypoint3 == null)
        {
            Handles.Label(transform.position + new Vector3(0, 3, 0), "ERROR, waypoints not set");
        }

        if (thinking == true)
        {
            Handles.Label(transform.position + new Vector3(0, 6, 0), "Thinking...");
        }
    }
#endif
    #endregion

    void Start()
    {
        waypoint = waypoint1.transform.position;
    }

    private void Awake()
    {
        if(player == null) player = GameObject.Find("Player").transform;
        playerHide = GameObject.Find("Player").GetComponent<CrouchToHide_Script>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //The timer for the corrupted to stop at a waypoint and think for a bit
        //Subtracting "think" from the ingame timer every frame
        if (think > 0)
        {
            think -= Time.deltaTime;

            //Done thinking
            if (think <= 0)
            {
                //Lets the corrupted continue to move
                agent.isStopped = false;
                thinking = false;
            }
        }

        //Timer for the grab stun
        if (stunTimer > 0) stunTimer -= Time.deltaTime;

        if (isHoldingPlayer == false)
        {
            corrAnimationScript.CorruptedAnimation(C_Animation.corruptedAnimationState.ungrab);
            //Checking for sight range (and eventually atttack range)
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            //playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange)
            {
                Patroling();
            }

            //only chase player if in range, not hiding, not in lock, and enemy not stunned
            if (playerInSightRange && playerHide.hiding == false && stunTimer <= 0)
            {
                //When player is busy in a lock, go back to patroling 
                if(PlayerMovement.playerBusy == true && !chasing)
                {
                    Patroling();
                }
                else ChasePlayer();
            }

            if (chasing && playerHide.hiding) StopMoving();
        }

        //Override movement for when player is grabbed, and enemy look at destination
        else if (isHoldingPlayer == true)
        {
            corrAnimationScript.CorruptedAnimation(C_Animation.corruptedAnimationState.grab);
            TakePlayerToEgg();
        }
    }

    #region Pathing
    private void Patroling()
    {
        //setting speed at beggning for when the player is busy in the lock
        //and so the enemy won't be pathing super fast
        agent.speed = 3;
        corrAnimationScript.CorruptedAnimation(C_Animation.corruptedAnimationState.walk);

        //Searches for a new main waypoint, once they reach their current waypoint
        if (!walkPointSet1)
        {
            //If corrupted reached the main waypoint, roam to a random point around that waypoint
            if (roam)
            {
                Roam();
            }
            NewWaypoint();
        }

        //Once main waypoint is found set desination to that waypoint
        if (walkPointSet1)
        {
            agent.SetDestination(walkPoint1);
        }

        //Once new roam point is found, set destination to that roam point
        if (walkPointSet2)
        {
            agent.SetDestination(walkPoint2);
        }

        //calculate the distance between the main waypoint or roam point and the corrupted's current position
        Vector3 distanceToWalkPoint1 = transform.position - walkPoint1;
        Vector3 distanceToWalkPoint2 = transform.position - walkPoint2;

        //If main waypoint is reached, think and then roam
        if (distanceToWalkPoint1.magnitude < 1f)
        {
            Think();
            roam = true;
            walkPointSet1 = false;
        }

        //If roam walkpoint is reached, go to new main waypoint
        if (distanceToWalkPoint2.magnitude < 1f)
        {
            //Think();
            roam = false;
            walkPointSet2 = false;
        }
    }

    //Sets the new wapoint from NewWaypoint() to walkpoint1, and sent to patrol function to set new destination
    private void SearchWalkPoint(Vector3 waypoint)
    {
        walkPoint1 = waypoint;

        //checks is the waypoint is not wonky or in some crazy place
        if (Physics.Raycast(walkPoint1, -transform.up, 2f, whatIsGround))
        {
            walkPointSet1 = true;
        }
    }

    private void Roam()
    {
        //Calculates random point in range  of the current waypoint
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        //sets the random transforms to the new walkpoint2 variable, to then be sent to patrol and set the new destination
        walkPoint2 = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //checks is the waypoint is not wonky or in some crazy place
        if (Physics.Raycast(walkPoint2, -transform.up, 2f, whatIsGround))
        {
            walkPointSet2 = true;
        }
    }

    private void NewWaypoint()
    {
        //swaps waypoints from the one that was just reached with another from the list of waypoints on the corrupted game object
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
        //sets the think timer to a randome range between 1 and 4, and will pause the corrupted movement for that amount of time
        think = Random.Range(1, 4);

        //stops the movement of the corrupted
        agent.isStopped = true;
        thinking = true;
    }
    #endregion

    #region States
    private void TakePlayerToEgg()
    {
        //transform.LookAt(capturePoint.transform.position + transform.position);
        //transform.position = Vector3.MoveTowards(transform.position, capturePoint.transform.position - (playerSocket.transform.position - transform.position), speed * Time.deltaTime);

        agent.SetDestination(capturePoint.transform.position - (playerSocket.transform.position - transform.position));
        player.transform.position = Vector3.MoveTowards(player.transform.position, playerSocket.transform.position, playerspeed * Time.deltaTime);
    }

    private void ChasePlayer()
    {
        corrAnimationScript.CorruptedAnimation(C_Animation.corruptedAnimationState.walk);

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
        stunTimer = grabBreakoutStun;
        isHoldingPlayer = false;
    }
    #endregion

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LyreTrigger")
        {
            Debug.Log("Lyre Collider stunned corrupted");
            StunThisEnemy();
        }

        if (other.gameObject.tag == "Player" && stunTimer <= 0)
        {
            isHoldingPlayer = true;

            Struggle struggleScript = other.gameObject.GetComponent<Struggle>();
            struggleScript.StartStruggling(this.gameObject.GetComponent<Corrupted_Script>());

            Debug.Log("Corrupted grabbed the player.");
        }

        if(other.gameObject.tag == "Table")
        {
            //Destroy(other.gameObject);
            //other.GetComponent<Rigidbody>().AddForce(0, 3, -3 * thrust, ForceMode.Impulse);
            enemyRanIntoTable = true;
        }
    }

    #region Drawn Gizmos
    public void OnDrawGizmosSelected()
    {
        //detection fields for seing and attacking the player
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        //shows the next waypioint the corrupted will take
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(walkPoint1, 1);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(walkPoint2, 1);

        //showing the path the corrupted with mainly take
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(waypoint1.transform.position, waypoint2.transform.position);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(waypoint2.transform.position, waypoint3.transform.position);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(waypoint3.transform.position, waypoint1.transform.position);
    }
    #endregion
}