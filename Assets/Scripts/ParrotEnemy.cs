using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ParrotEnemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask GroundLayerM, PlayerM;

    private Vector3 groundLayer;

    //int health = ;
    //int damage = ;

    

    //Patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attack
    public float timeBetweenAttacks;
    bool alreadyAttack;
    public GameObject projectile;

    //Motion state
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //To find obj
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        //Define new vector 3
        groundLayer = new Vector3(0, 2f, 0);
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerScript>().transform;
    }

    private void Update()
    {
        //Check for player in ranges
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, PlayerM);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, PlayerM);

        //When is enemy patrolling
        if (!playerInAttackRange && !playerInSightRange) Patrol();

        //When will enemy chase player
        if (!playerInAttackRange && playerInSightRange) Chase();

        //When will enemy chase player
        if (playerInAttackRange && playerInSightRange) Attack(); 


    }


    //Patrol
    private void Patrol()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude <= 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        //Define walkPoint
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //Check if on ground
        if (Physics.Raycast(walkPoint - transform.up, groundLayer, GroundLayerM))
            walkPointSet = true;
    }

    //Chase 
    private void Chase()
    {
        agent.SetDestination(player.position);
    }

    //Attack
    private void Attack()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttack)
        {

            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            alreadyAttack = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

        }
    }

    //Reset the attack by enemy
    private void ResetAttack()
    {
        alreadyAttack = false;
    }

    //Enemy being attacked
    private void OnCollisionEnter(Collision collision)
    {
        
        //If enemy gets hit by player bullet
        if (collision.gameObject.tag == "PlayerBullet")
        {
            health += damage;
            DestroyEnemy();
        }
    }

    //Enemy died
    private void DestroyEnemy()
    {
        if (health <= 0)
        {
            //Destroy(GameObject.FindWithTag("Enemy"));
            Destroy(gameObject);
        }
    }
}
