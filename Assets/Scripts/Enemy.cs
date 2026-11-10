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
    Vector3 SearchPosition;

    state stateOfEnemy;
    Vector3 startPos;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        if (stateOfEnemy == state.Patrol)
            agent.SetDestination(patrolPoints[currentPoint].position);

        agent.speed = AnimalData.Stats[animal].moveSpeed;
        startPos = transform.position;
    }
    public void ResetEnemy()
    {
        stateOfEnemy = state.Patrol;
        currentPoint = 0;
        transform.position = startPos;
        gameObject.SetActive(true);
    }
    //agent.setdestiantion işi daha optimize olacak şekilde her kare değil de arada bir çağrılacak şekiled ayarlanacak
    void Update()
    {
        if (agent.pathPending)
            return;
        if (stateOfEnemy == state.Patrol)
        {
            ListenForPlayer();
            LookForPlayer();

            Patrol();
        }
        if (stateOfEnemy == state.Search)
        {
            ListenForPlayer();
            LookForPlayer();

            SearchForPlayer();
        }
        if (stateOfEnemy == state.Chase)
        {
            ListenForPlayer();
            LookForPlayer();

            ChasePlayer();
        }

    }
    void Patrol()
    {
        agent.SetDestination(patrolPoints[currentPoint].position);
        if (agent.isActiveAndEnabled && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Count;
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
    }
    //oyuncunun tespit mesafesi ve çıkardığı seslerin mesafesi hayvana bağlı

    void ListenForPlayer()
    {
        float dist = Vector3.Distance(PlayerController.Instance.transform.position, transform.position);
        float value = dist * AnimalData.Stats[PlayerController.Instance.animal].footStepLoudness;

        if (PlayerController.Instance.makingNoise && value < AnimalData.Stats[animal].hearDistance)
        {
            stateOfEnemy = state.Search;
            SearchPosition = PlayerController.Instance.transform.position;
        }
        //oyuncunun aksiyonları buradan kontrol edilecek
        //oyuncu her aksiyon yaptığında buradan izlenecek gibi bişey olucak
        //oyuncu bişey yapmadıkça çalışmayacak
    }

    void LookForPlayer()
    {
        //TODO görüp , durumu eğer uzaktaysa aramaya ; yakındaysa takibe değiştirme mantığı eklenecek,
        // açılar ; uzaklık ve raycast kullanılacaktır.
        // hayvan istatistiklerinden yararlanılacaktır


    }
    float spinCounter = 0.0f;
    void SearchForPlayer()
    {
        //oyuncu eğer görüldü veya ses çıkardıysa en son bilinen yerine doğru gidiliecek
        //daha sonra etrafında dönücek
        //belki yakındaki biryere gidebilir
        //ya da oyuncunun en sonki gidişatına göre devam edebilir
        agent.SetDestination(SearchPosition);
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            spinCounter += Time.deltaTime;
            transform.Rotate(new(0, 0, 5));
            if (spinCounter > 1.0f)
            {
                stateOfEnemy = state.Patrol;
                transform.rotation = Quaternion.identity;
                spinCounter = 0;
            }
            //birkaç saniye dön sonra geri yoluna devam et
        }
    }

    void ChasePlayer()
    {
        //takibi bırakma koşulları görüşün kaybolması 
        //search moduna geçicek
        agent.SetDestination(PlayerController.Instance.transform.position);
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            PlayerController.Instance.Die();
        }
        //direk oyuncuya gidecek
    }

    public EnemyInfoToSend Explode()
    {
        //burada bu obje kapatılacak , fakat oyuncu bölüm sırasında ölürse deloadlanıp prefabdan 
        //tekra yükleneceğinden dolayı tekrar açılmasına gerek yoktur,
        //direk yok edlse daha iyi gibi
        gameObject.SetActive(false);
        //Destroy(gameObject,5);
        return new EnemyInfoToSend(spriteObject, animal);
    }
    private enum state
    {
        Patrol,
        Search,
        Chase
    }
    
}

public struct EnemyInfoToSend
{
    public GameObject gameObjectOfTheEnemySprite;
    public Animals typeOfAnimalOfTheEnemy;

    public EnemyInfoToSend(GameObject gameObject, Animals animal) : this()
    {
        gameObjectOfTheEnemySprite = gameObject;
        typeOfAnimalOfTheEnemy = animal;
    }
}
