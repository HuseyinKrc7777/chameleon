using System.Collections.Generic;
using ChameleonGame;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Room Room;
    public List<Transform> patrolPoints = new();
    public int currentPoint = 0;
    [SerializeField] Animals animal;
    [SerializeField] public GameObject spriteObject;
    NavMeshAgent agent;
    void Start()
    {
        if(animal == Animals.NONE)
            Debug.LogError("ANIMAL NOT SET");
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.SetDestination(patrolPoints[currentPoint].position);
    }

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

    public EnemyInfoToSend Explode()
    {
        //burada bu obje kapatılacak , fakat oyuncu bölüm sırasında ölürse deloadlanıp prefabdan 
        //tekra yükleneceğinden dolayı tekrar açılmasına gerek yoktur,
        //direk yok edlse daha iyi gibi
        gameObject.SetActive(false);
        //Destroy(gameObject,5);
        return new EnemyInfoToSend(spriteObject,animal);
    }
}

public struct EnemyInfoToSend
{
    public GameObject gameObjectOfTheEnemySprite;
    public Animals typeOfAnimalOfTheEnemy;

    public EnemyInfoToSend(GameObject gameObject,Animals animal) : this()
    {
        gameObjectOfTheEnemySprite = gameObject;
        typeOfAnimalOfTheEnemy = animal;
    }
}
