using UnityEngine;

public class TowerAI : MonoBehaviour
{
    public enum TowerState {
        Patrol,
        Attack,
        Die
    }
    public TowerState currentState = TowerState.Patrol;

    [Header("Patrol Settings")]
    public Transform turret;
    public float rotationSpeed = 30f;
    public float maxRotationAngle = 90f;
    public float detectionRange = 10f;

    [Header("Attack Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    //rate is firing per second
    public float fireRate = 2f;
    float fireCooldown = 0;

    Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case TowerState.Patrol:
                Patrol();
                break;
            case TowerState.Attack:
                Attack();
                break;
            case TowerState.Die:
                Die();
                break;
        }
    }

    void Patrol()
    {
        Debug.Log("Patrolling <_< ... >_>");

        //turret.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // patrolling
        // rotation back and forth
        float angle = Mathf.PingPong(rotationSpeed * Time.time, maxRotationAngle * 2) - maxRotationAngle;
        turret.localRotation = Quaternion.Euler(0, angle, 0);

        // see if you can detect enemies
        LookForEnemies();
    }

    void Attack()
    {
        Debug.Log("ATTACKING >:(");

        if (target == null || Vector3.Distance(transform.position, target.position) > detectionRange)
        {
            target = null;
            currentState = TowerState.Patrol;
            return;
        }
        
        //face the enemy
        turret.LookAt(target);

        //check cooldown - can we shoot?
        if (fireCooldown <= 0)
        {
            Shoot();
            fireCooldown = 1 / fireRate;
            Debug.Log("fireCooldown " + fireCooldown);
        }

        fireCooldown -= Time.deltaTime;
    }

    void Die()
    {
        Debug.Log("dead... X_X");
    }

    void LookForEnemies()
    {
        //get array of all objects with colliders that overlap with this imaginary sphere
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        //find nearest enemy, if it exists
        foreach(Collider curr in colliders)
        {
            if (curr.CompareTag("Enemy"))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, curr.transform.position);

                if (distanceToEnemy < shortestDistance) {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = curr.transform;
                }
            }
        }

        if (nearestEnemy)
        {
            target = nearestEnemy;
            Debug.Log("Target detected: " + target.name);
            currentState = TowerState.Attack;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        
    }
}
