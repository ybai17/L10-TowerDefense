using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float bulletSpeed = 20f;
    public float rotationSpeed = 5f;
    public float lifetime = 5f; //seconds
    public int damage = 10;

    public GameObject bulletHitVFXPrefab;

    private Rigidbody rb;
    private Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (!target)
            return;

        //direction to target
        Vector3 direction = (target.position - transform.position).normalized;

        //smooth rotation
        Quaternion rotationToTarget = Quaternion.LookRotation(target.position);
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                                rotationToTarget,
                                                rotationSpeed * Time.fixedDeltaTime);

        //move bullet forward
        rb.linearVelocity = transform.forward * bulletSpeed;
    }

    public void SetTarget(Transform currentTarget)
    {
        target = currentTarget;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet HIT " + collision.transform.name);

        if (bulletHitVFXPrefab)
        {
            Vector3 collisionPoint = collision.GetContact(0).point;
            Instantiate(bulletHitVFXPrefab, collisionPoint, Quaternion.identity);
        }
    }
}
