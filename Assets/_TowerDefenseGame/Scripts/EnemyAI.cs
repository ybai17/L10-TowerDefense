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
    public GameObject buildFXPrefab;
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

    bool isDying;

    Quaternion originalTurretRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(targetBase.position);
        //this also works
        //agent.destination = targetBase.position;

        isDying = false;

        originalTurretRotation = turret.localRotation;
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

        turret.localRotation = Quaternion.Slerp(turret.localRotation, originalTurretRotation, rotationSpeed * Time.deltaTime);
    }

    void Attack()
    {
        //check if we can attack
        if (attackTarget == null || Vector3.Distance(transform.position, attackTarget.position) > detectionRange)
        {
            attackTarget = null;
            currentState = EnemyState.Navigate;
            return;
        }

        //look at tower
        Vector3 direction = attackTarget.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        turret.rotation = Quaternion.Slerp(turret.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        //can we shoot?
        if (fireCooldown <= 0)
        {
            Shoot();
            fireCooldown = 1f / fireRate;
        }
        fireCooldown -= Time.deltaTime;
    }
    
    void Die()
    {
        if (isDying)
        {
            return;
        }

        Debug.Log("Enemy down");
        agent.isStopped = true;
        if (destroyFXPrefab)
        {
            Instantiate(destroyFXPrefab, transform.position, transform.rotation);
            Destroy(gameObject, 1);

            isDying = true;
        }
    }

    void FindNearestTower()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        Transform nearestTower = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider curr in colliders)
        {
            if (curr.CompareTag("Tower"))
            {
                float currDistance = Vector3.Distance(curr.gameObject.transform.position, transform.position);
                if (currDistance < shortestDistance)
                {
                    nearestTower = curr.gameObject.transform;
                    shortestDistance = currDistance;
                }
            }
        }

        if (nearestTower)
        {
            attackTarget = nearestTower;
            Debug.Log("Enemy is targeting: " + attackTarget.name);
            currentState = EnemyState.Attack;
            return;
        }
    }

    void Shoot()
    {
        var bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        bulletBehavior.SetTarget(attackTarget);
    }

    void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            currentState = EnemyState.Die;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("TowerBullet"))
        {
            BulletBehavior bulletBehavior = collision.gameObject.GetComponent<BulletBehavior>();
            if (bulletBehavior)
            {
                int damage = bulletBehavior.GetDamageValue();
                TakeDamage(damage);
                Debug.Log("ENEMY took " + damage + " damage");
            } else
            {
                Debug.Log("No damage taken from bullet");
            }
        }   
    }
}
