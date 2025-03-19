using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent agent;

    public Transform targetBase;

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
        
    }
}
