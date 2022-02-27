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
    
    public LayerMask whatIsGround, whatIsPlayer;

    //corrupted patrolling code

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public Transform[] points;
    int current;
    public float speed;
    public float playerspeed;

    private float timer;

    //state of corrupted

    public float sightRange;
    private bool playerInSightRange;

    public float attackRange;
    private bool playerInAttackRange;

    [SerializeField] private float grabBreakoutStun = 3;


    private void OnDrawGizmos()
    {
        Handles.Label(transform.position, "ERROR, no egg targeted for return");
    }

    void Start()
    {
        current = 0;
        //speed = 3;
    }

    private void Awake()
    {
         player = GameObject.Find("Player").transform;
         agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (timer > 0)  timer -= Time.deltaTime;

        if (isHoldingPlayer == false)
        {
            //Checking for sight range (and eventually atttack range)
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange) Patroling();

            //only chase player if in range, not hiding, and not stunned
            if (playerInSightRange && player.GetComponent<CrouchToHide_Script>().hiding == false && timer <= 0) ChasePlayer();

            //if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }

        //Override movement for when player is grabbed, and enemy look at destination
        if(isHoldingPlayer == true)
        {
            TakePlayerToEgg();
        }
            
        
        
       /**
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            speed = 2;
            
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            speed = 5; 
        }

        if (Input.GetButtonDown("Fire1"))
        {
            speed = 2;
        }

         if (Input.GetButtonUp("Fire1"))
        {
            speed = 5;
        }**/
        
    }
        private void Patroling()
        {
             if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
            {
                agent.SetDestination(walkPoint);
            }
               

             Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //If WalkPoint is reached
        if (distanceToWalkPoint.magnitude < 1f)
                 walkPointSet = false;
        }

         private void SearchWalkPoint()
        {
            //Calculates random point in range
             float randomZ = Random.Range(-walkPointRange, walkPointRange);
             float randomX = Random.Range(-walkPointRange, walkPointRange);

             walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

             if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
                 walkPointSet = true;
         }

        private void TakePlayerToEgg()
        {
            //transform.LookAt(capturePoint.transform.position + transform.position);
            //transform.position = Vector3.MoveTowards(transform.position, capturePoint.transform.position - (playerSocket.transform.position - transform.position), speed * Time.deltaTime);

            agent.SetDestination(capturePoint.transform.position - (playerSocket.transform.position - transform.position));

            player.transform.position = Vector3.MoveTowards(player.transform.position, playerSocket.transform.position, playerspeed * Time.deltaTime);
        }

         private void ChasePlayer()
         {
             agent.speed = 10;

             agent.SetDestination(player.position);
             //transform.LookAt(player.position + transform.position);
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


    //when Corrupted is no longer colliding with lyre collider sphere
    /**void OnCollisionExit(Collision other)
    {
        speed = 3;
    }**/
}
