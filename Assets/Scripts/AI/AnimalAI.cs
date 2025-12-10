using UnityEngine;
using UnityEngine.AI;

public enum AnimalType
{
    Hostile,
    Docile
}


public class AnimalAI : MonoBehaviour
{
    public AnimalType animalType = AnimalType.Docile;
    public float detectionRange = 10;
    public float fleeRange = 8;
    public float attackRange = 2;
    public float attackDamage = 10;
    public float attackCooldown = 1;

    public float patrolRadius = 10;
    public float patrolPointTolerance = 1;

    private NavMeshAgent agent;
    private Transform player;
    private Player playerScript;

    private float nextAttackTime = 0;
    private Vector3 patrolPoint;
    private Animator animator;

    private enum State
    {
        Patrol,
        Flee,
        Chase,
        Attack
    }

    private State currentState;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.GetComponent<Player>();

        SetNewPatrolPoint();
        currentState = State.Patrol;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                if (animalType == AnimalType.Docile && distance < fleeRange)
                {
                    currentState = State.Flee;
                }
                else if (animalType == AnimalType.Hostile && distance < detectionRange)
                {
                    currentState = State.Chase;
                }
                break;
            case State.Chase:
                Chase();
                if (distance < attackRange)
                {
                    currentState = State.Attack;
                }
                else if (distance > detectionRange * 1.5f)
                {
                    currentState = State.Patrol;
                }
                break;
            case State.Flee:
                Flee();
                if (distance > fleeRange * 1.5f)
                {
                    currentState = State.Patrol;
                }
                break;
            case State.Attack:
                Attack(distance);
                if (distance > attackRange)
                {
                    currentState = State.Chase;
                }
                break;
        }

    }

    private void Patrol()
    {
        if (Vector3.Distance(transform.position, patrolPoint) < patrolPointTolerance)
        {
            SetNewPatrolPoint();
        }
        agent.SetDestination(patrolPoint);
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
    }

    private void Flee()
    {
        Vector3 direction = (transform.position - player.position).normalized;
        Vector3 fleePosition = transform.position + direction * patrolRadius;

        agent.SetDestination(fleePosition);
    }

    private void Attack(float distance)
    {
        agent.SetDestination(transform.position);

        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            playerScript.ModifyHealth(-attackDamage);
        }
    }

    private void SetNewPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas);

        patrolPoint = hit.position;
    }
}
