using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float chaseRange = 10f;
    public float stopChaseRange = 15f;
    public float attackRange = 2f; 
    public float attackCooldown = 2f;
    public float distanceToPlayer;

    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;
    public float waitTimeAtPatrolPoint = 2f;

    public int maxHealth = 3;
    private int currentHealth;
    public int damageAmount = 10;

    private NavMeshAgent navMeshAgent;
    private Transform player;
    private bool isChasing = false;
    private int currentPatrolIndex;
    private float waitTimer;
    private float attackTimer;

    private Renderer enemyRenderer;
    private Color originalColor; //store the original enemy color

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentPatrolIndex = 0; // start at the first patrol point
        navMeshAgent.speed = patrolSpeed;
        currentHealth = maxHealth; // initialize health
        attackTimer = 0f;

        enemyRenderer = GetComponent<Renderer>();
        originalColor = enemyRenderer.material.color; //store the original enemy color
    }

    void Update()
    {
        // find the player (will keep updating until player is found)
        if (player == null)
        {
            FindPlayer();
            return;
        }

        // calculates the distance between the enemy and the player
        distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // chase if within chase range and not already chasing
        if (distanceToPlayer <= chaseRange && !isChasing)
        {
            StartChasing();
        }
        // stop chasing if the player moves outside the stop chase range
        else if (distanceToPlayer > stopChaseRange && isChasing)
        {
            StopChasing();
        }

        // set the destination to the players position if enemy is chasing
        if (isChasing)
        {
            navMeshAgent.SetDestination(player.position);

            // if within attack range, attack the player
            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
        }
        else
        {
            Patrol();
        }

        attackTimer += Time.deltaTime;
    }

    private void Patrol()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            waitTimer += Time.deltaTime; // wait at the patrol point

            if (waitTimer >= waitTimeAtPatrolPoint)
            {
                // move to the next patrol point
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // Loop back to the start
                navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position); // Set the new destination
                waitTimer = 0; // reset the wait timer
            }
        }
        else
        {
            waitTimer = 0; // resets the wait timer if moving
        }
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            //Debug.Log("Player found by EnemyAI");
        }
        else
        {
            Debug.LogError("Player not found by EnemyAI");
        }
    }

    private void StartChasing()
    {
        isChasing = true;
        navMeshAgent.isStopped = false;
        Debug.Log("Enemy has started chasing the player");
    }

    private void StopChasing()
    {
        isChasing = false;
        navMeshAgent.isStopped = true;
        Debug.Log("Enemy has stopped chasing the player");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Enemy took {damage} damage. Current health: {currentHealth}");

        //flash red
        StartCoroutine(FlashRed());

        // calls the die method if health hits zero
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashRed()
    {
        //change the enemy's color to red
        enemyRenderer.material.color = Color.red;

        //wait for half a second
        yield return new WaitForSeconds(0.5f);

        //change back to the original color
        enemyRenderer.material.color = originalColor;
    }

    private void AttackPlayer()
    {
        if (attackTimer >= attackCooldown)
        {
            PlayerBehavior playerBehavior = player.GetComponent<PlayerBehavior>();

            if (playerBehavior != null)
            {
                playerBehavior.TakeDamage(damageAmount);
                Debug.Log($"Player took {damageAmount} damage from the enemy!");
            }

            attackTimer = 0f; // resets timer
        }
    }

    private void Die()
    {
        Debug.Log("Enemy has died.");
        Destroy(gameObject);
    }
}