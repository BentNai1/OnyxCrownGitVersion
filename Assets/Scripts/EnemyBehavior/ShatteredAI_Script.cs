using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShatteredAI_Script : MonoBehaviour
{
    private NavMeshAgent agent;
    
    private Transform player;

    public LayerMask whatIsPlayer;

    //Pathing
    private Vector3 walkPoint;
    bool walkPointSet = false;
    private Vector3 patrolP1;

    [Header("Movement")]
    public GameObject patrolP2;
    bool isP1Current = false;
    [HideInInspector]
    public GameObject Player;
    //[Tooltip("Distance to move before changing direction")]
    //public float walkPointRange;
    //[Tooltip("Time to pause before changing direction.")]
    //public float waitTime = 1f;
    //private bool forcingPath = false;
    //private bool pathCalled = true;

    //Sensing
    [Header("Sensing")]
    [Tooltip("Activates enemy and spawns sensors when player is within this range")]
    public float sightRange;
    [Tooltip("Should be set to \"SensingOrb\"")]
    public GameObject SensingOrb;
    
    private bool orbDispensed = false;
    //[Tooltip("Time between spawning sensors while player is within range")]
    //public float timeBetweenOrbs = 1f;
    [Tooltip("Starting size of sensing orb")]
    public float orbScale = 1;
    [Tooltip("How fast orb grows while colliding with player")]
    public float growRate = 5;
    [Tooltip("Shattered will chase player when orb reaches this size")]
    public float orbMax = 20;
    [Tooltip("How fast orb shrinks while not colliding with player")]
    public float shrinkRate = 0.5f;
    [Tooltip("Orb will disappear at this size")]
    public float orbMin = 0.1f;

    //Attacking
    [Header("Attacking")]
    //[Tooltip("Attack damage")]
    //public float damage;
    [Tooltip("The character model that appears when attacking")]
    public GameObject selfModel;
    [Tooltip("Time before enemy can damage player again")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    [Tooltip("Time until enemy gives up on chasing the player")]
    public float attackTimeout;
    private bool attackTimerSet = false;
    [HideInInspector]
    public bool activelyAttacking;
    [Tooltip("If true, this enemy will be destroyed after damaging the player")]
    public bool dieOnAttack = false;

    //States
    private bool dormant = true;
    [Tooltip("How close the player has to be to get damaged")]
    public float attackRange;
    [HideInInspector]
    public bool playerInSightRange, playerInAttackRange;

    private void Start()
    {
        //Initialize references
        Player = GameObject.FindWithTag("Player");
        player = Player.transform;
        agent = GetComponent<NavMeshAgent>();
        walkPoint = patrolP2.transform.position;
        patrolP1 = gameObject.transform.position;

        //Calculate and begin patrol path
        agent.SetDestination(walkPoint);
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(walkPoint, path);
        if (path.status != NavMeshPathStatus.PathComplete)
        {
            Debug.Log("WARNING: Shattered could not find path to waypoint 'patrolP2'");
        }
    }

    private void Update()
    {
        //Check if player is in sight range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        //Check if player is in attack radius
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        Patrolling();

        //Attempt to attack player
        if (activelyAttacking)
        {
            //Check if player is hiding before attacking
            if (Player.GetComponent<CrouchToHide_Script>().hiding == false)
            {
                AttackPlayer();

                if (!attackTimerSet)
                {
                    Invoke(nameof(StopAttacking), attackTimeout);
                    attackTimerSet = true;
                }
            }
            else
            {
                print("Shattered can no longer see player (crouched)");
                StopAttacking();
            }
        }
        else
        {
            Sensing();
        }

        if (playerInAttackRange)
        {
            if (!alreadyAttacked) //Hurts player if direct contact is made
            {
                //Damage
                Player.GetComponent<Player_Health>().DealDamageToPlayer(Player.GetComponent<Player_Health>().playerCurrentHealth);
                print("Damaged player!");

                if (dieOnAttack)
                {
                    Destroy(gameObject);
                }

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }

            activelyAttacking = false;

            GoToPatrolPoint();
        }
    }

    private void Patrolling()
    {
        //Walkpoint reached
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (!activelyAttacking && distanceToWalkPoint.magnitude < 1f)
        {
            //Switch walkpoint to opposite patrol point
            isP1Current = !isP1Current;
            GoToPatrolPoint();
        }
    }

    private void GoToPatrolPoint() //Sets destination to current patrol point
    {
        switch (isP1Current)
        {
            case true:
                walkPoint = patrolP1;
                break;
            case false:
                walkPoint = patrolP2.transform.position;
                break;
        }
        agent.SetDestination(walkPoint);
    }

    //OBSOLETE
    //private void StopMoving()
    //{
    //    walkPointSet = false;
    //    agent.SetDestination(gameObject.transform.position);
    //    if (!pathCalled)
    //    {
    //        pathCalled = true;
    //    }
    //}

    //OBSOLETE
    //private void SearchWalkPoint()
    //{
    //    NavMeshPath path = new NavMeshPath();

    //    RandomWalkPoint();

    //    agent.CalculatePath(walkPoint, path);

    //    if (path.status == NavMeshPathStatus.PathComplete)
    //    {
    //        walkPointSet = true;
    //        pathCalled = false;
    //    }
    //    else
    //    {
    //        Invoke(nameof(SearchWalkPoint), waitTime);
    //    }
    //}

    //OBSOLETE
    //private void RandomWalkPoint()
    //{
    //    //Calculate random point in range
    //    float randomZ = Random.Range(-walkPointRange, walkPointRange);
    //    float randomX = Random.Range(-walkPointRange, walkPointRange);

    //    walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    //}

    //OBSOLETE
    //This function is used to move the AI when it's pathfinding gets stuck (like when inside the player)
    //private void ForceWalkPoint()
    //{
    //    forcingPath = true;
    //    RandomWalkPoint();
    //    walkPointSet = true;
    //}

    private void Sensing()
    {
        if (playerInSightRange && !orbDispensed)
        {
            print("Dispensing");
            GameObject dispencedOrb = Instantiate(SensingOrb, Player.transform.position + Vector3.up*3, Quaternion.identity);
            orbDispensed = true;
            dispencedOrb.GetComponent<SensingOrb_Script>().Creator = gameObject;
            Invoke(nameof(ResetOrb), 1f);
        }
    }

    private void ResetOrb()
    {
        print("reset orb");
        orbDispensed = false;
    }

    private void AttackPlayer()
    {
        //Show self
        selfModel.SetActive(true);

        //Face player
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude > 2) //Distance from player is checked to prevent the shattered from tilting up or down when close
        {
            transform.LookAt(player);
        }

        //Chase player
        agent.SetDestination(player.position);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void StopAttacking()
    {
        //hide self
        selfModel.SetActive(false);

        //stop attacking
        activelyAttacking = false;

        //stop moving
        agent.SetDestination(gameObject.transform.position);

        GoToPatrolPoint();
    }
    
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(walkPoint, 1);
    }
}