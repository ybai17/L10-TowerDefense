using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState {
        Navigate,
        Attack,
        Die
    }

    [Header("General Settings")]
    public Transform targetBase;
    public EnemyState currentState = EnemyState.Navigate;

    NavMeshAgent agent;

    [Header("Navigation Settings")]
    public Transform turret;
    public float rotationSpeed = 30f;
    public float detectionRange = 10f;

    [Header("Attack Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    //rate is firing per second
    public float fireRate = 2f;
    float fireCooldown = 0;
    Transform attackTarget;

    [Header("Die Settings")]
    public int health = 100;
    public GameObject destroyFXPrefab;

    [Header("General Settings")]
    public GameObject buildFXPrefab;
    bool isDying;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(targetBase.position);
        //this also works
        //agent.destination = targetBase.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState) {
            case EnemyState.Navigate:
                Navigate();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }

    void Navigate()
    {
        agent.SetDestination(targetBase.position);

        FindNearestTower();
    }

    void Attack()
    {

    }
    
    void Die()
    {

    }

    void FindNearestTower()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        Transform nearestTower = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider curr in colliders)
        {
            float currDistance = Vector3.Distance(curr.gameObject.transform.position, transform.position);
            if (currDistance < shortestDistance)
            {
                nearestTower = curr.gameObject.transform;
                shortestDistance = currDistance;
            }
        }

        if (nearestTower)
        {
            attackTarget = nearestTower;
            Debug.Log("Buggy targeting: " + attackTarget.name);
            currentState = EnemyState.Attack;
        }
    }
}
