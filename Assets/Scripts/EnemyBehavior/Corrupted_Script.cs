using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Corrupted_Script : MonoBehaviour
{
    public bool isHoldingPlayer;

    public NavMeshAgent agent;
    public GameObject capturePoint;
    public GameObject playerSocket;

    public Transform player;
    
    public LayerMask whatIsGround, whatIsPlayer;

    //corrupted patrolling code

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public Transform[] points;
    int current;
    public float speed;
    public float playerspeed;
    public float timer;

    //state of corrupted

    public float sightRange;
    public bool playerInSightRange;

    public float attackRange;
    public bool playerInAttackRange;


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
        //timer -= Time.deltaTime;

        if (isHoldingPlayer == false)
        {
            //Checking for sight range (and eventually atttack range)
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange) Patroling();
            if (playerInSightRange && player.GetComponent<CrouchToHide_Script>().hiding == false) ChasePlayer();
            //if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }

        //Override movement for when player is grabbed, and enemy look at destination
        if(isHoldingPlayer == true)
        {
            transform.LookAt(capturePoint.transform.position + transform.position);
            transform.position = Vector3.MoveTowards(transform.position, capturePoint.transform.position - (playerSocket.transform.position - transform.position), speed * Time.deltaTime);
            player.transform.position = Vector3.MoveTowards(player.transform.position, playerSocket.transform.position, playerspeed * Time.deltaTime);
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

         private void ChasePlayer()
         {
             agent.speed = 10;
             agent.SetDestination(player.position);
             transform.LookAt(player.position + transform.position);
         }

         private void AttackPlayer()
         {
         }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerMovement playerScript = other.gameObject.GetComponent<PlayerMovement>();
            playerScript.isGrabbed = true;
            

            Struggle struggleScript = other.gameObject.GetComponent<Struggle>();
            struggleScript.isStruggling = true;


            struggleScript.capturePoint = this.gameObject;
            
            struggleScript.speedFromEnemy = speed;

            isHoldingPlayer = true;
        }

        if(other.gameObject.tag == "lyreCollider")
        {
            //-------------------------------------might wanna just completley imobolize them having trouble getting the speed value i want and its pretty small ~N
            //speed = 1;
        }
    }

    //when Corrupted is no longer colliding with lyre collider sphere
    /**void OnCollisionExit(Collision other)
    {
        speed = 3;
    }**/
}
