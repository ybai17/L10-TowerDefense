using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    public int baseDamageValue = 10;

    NavMeshAgent agent;

    [Header("Navigation Settings")]
    public Transform turret;
    public float rotationSpeed = 30f;
    public float detectionRange = 10f;

    [Header("Attack Settings")]
    public bool canAttack = true;
    public GameObject projectilePrefab;
    public Transform firePoint;
    //rate is firing per second
    public float fireRate = 2f;
    float fireCooldown = 0;
    Transform attackTarget;

    [Header("Die Settings")]
    public int health = 100;
    public GameObject destroyFXPrefab;
    public Slider healthSlider;

    bool isDying;

    Quaternion originalTurretRotation;
    int maxHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (!targetBase)
        {
            targetBase = GameObject.FindGameObjectWithTag("Target").transform;

            if (!targetBase)
            {
                Debug.Log("No target base found for enemies");
                return;
            }
        }

        agent.SetDestination(targetBase.position);
        //this also works
        //agent.destination = targetBase.position;

        isDying = false;

        if (turret)
            originalTurretRotation = turret.localRotation;

        maxHealth = health;

        if (healthSlider)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState) {
            case EnemyState.Navigate:
                Navigate();
                break;
            case EnemyState.Attack:
                if (canAttack)
                    Attack();
                else
                    currentState = EnemyState.Navigate;
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }

    void Navigate()
    {
        //agent.SetDestination(targetBase.position);

        if (canAttack)
            FindNearestTower();

        if (turret)
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
            if (HasLineOfSight(attackTarget))
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
        if (!canAttack)
            return;
        
        var bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();

        if (bulletBehavior)
        {
            var targetTowerTurret = attackTarget.Find("Turret");
            if (targetTowerTurret)
                bulletBehavior.SetTarget(targetTowerTurret);
            else
                bulletBehavior.SetTarget(attackTarget);
        }
            
    }

    void TakeDamage(int damage)
    {
        health -= damage;

        if (healthSlider)
            healthSlider.value = health;

        if (health <= 0)
        {
            currentState = EnemyState.Die;
        }
    }

    bool HasLineOfSight(Transform target)
    {
        RaycastHit hit;
        Vector3 direction = (target.position - firePoint.position).normalized;

        if (Physics.Raycast(firePoint.position, direction, out hit, detectionRange))
        {
            if (hit.collider.gameObject.CompareTag("Tower"))
            {
                Debug.Log("Tower in LOS: " + hit.collider.name);
                return true;
            }
        }

        return false;
    }

    public int GetBaseDamageValue()
    {
        return baseDamageValue;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit by " + collision.transform.tag);
        if (collision.transform.CompareTag("Bullet"))
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
