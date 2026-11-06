using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Room Room;
    public List<Transform> patrolPoints = new();
    int currentPoint = 0;
    NavMeshAgent agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.SetDestination(patrolPoints[currentPoint].position);
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.pathPending)
            return;

        if (agent.isActiveAndEnabled && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Count;
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
    }

    public Transform Explode()
    {
        //burada bu obje kapatılacak , fakat oyuncu bölüm sırasında ölürse deloadlanıp prefabdan 
        //tekra yükleneceğinden dolayı tekrar açılmasına gerek yoktur,
        //direk yok edlse daha iyi gibi
        gameObject.SetActive(false);
        Destroy(gameObject,5);
        return transform.Find("Sprite");
    }
}
