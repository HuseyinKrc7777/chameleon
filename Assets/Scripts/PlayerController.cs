using UnityEngine;
using ChameleonGame;
using System;
using UnityEditor.Media;
using Unity.Cinemachine;
using Unity.VisualScripting;
public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 8.0f;
    PlayerInputs _input;
    Rigidbody2D _rigidbody;
    float _speed;
    public bool readyToCrossRooms = true;
    [SerializeField] private Room _currentRoom;
    [SerializeField] private LayerMask InteractLayers;

    [SerializeField] public Animals animal;
    [SerializeField] GameObject CurrentSprite;

    playerSave latestSave;

    public Room CurrentRoom
    {
        get => _currentRoom;
        set
        {
            _currentRoom = value;
            OnRoomChange?.Invoke(_currentRoom);
        }
    }
    public RoomLoader lastUsedLoader;
    [SerializeField] private Camera _cam;
    public event Action<Room> OnRoomChange;
    public static PlayerController Instance { get; private set; }
    public bool makingNoise = false;
    private void Awake()
    {
        OnRoomChange += ChangeCameraToAnotherRoom;
        OnRoomChange += SaveAfterRoomChange;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;


    }
    private void OnDestroy()
    {
        OnRoomChange -= ChangeCameraToAnotherRoom;
        OnRoomChange -= SaveAfterRoomChange;
        if (Instance == this)
            Instance = null;

    }

    void Start()
    {
        _input = GetComponent<PlayerInputs>();
        _rigidbody = GetComponent<Rigidbody2D>();
        animal = Animals.Octopus;
        MoveSpeed = AnimalData.Stats[animal].moveSpeed;
    }
    void Update()
    {
        Move();
        Interact();
        Menu();
    }
    public void Die()
    {
        LoadPlayer();
        CurrentRoom.ReloadRoom();
    }
    private void LateUpdate()
    {

    }
    private void Move()
    {
        float targetSpeed = _input.move == Vector2.zero ? 0.0f : MoveSpeed;
        _speed = Mathf.Lerp(_speed, targetSpeed, Time.deltaTime * 10.0f);
        if(_speed > 0.1f && !_input.abilty1)
            makingNoise = true;
        else
            makingNoise = false;
        Vector3 inputDirection = new(_input.move.x, _input.move.y, 0);
        Vector3 playerMotion = inputDirection * (_speed * Time.deltaTime);
        Vector3 pos = transform.position;

        transform.position = pos + playerMotion;
        //rigidbody ile de hareket sağlanabilir?


    }
    private void Menu()
    {
        if (_input.menu)
        {
            LoadPlayer();
            CurrentRoom.ReloadRoom();
            _input.menu = false;
        }
    }
    private void Interact()
    {
        if (_input.interact)
        {
            //TODO timeout eklenebilir
            Vector2 origin = transform.position;
            Vector2 direction = transform.up;

            float radius = 0.5f;
            float distance = 5f;

            RaycastHit2D hit = Physics2D.CircleCast(
                origin,
                radius,
                direction,
                distance,
                InteractLayers
            );

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    //Destroy(transform.Find("Sprite").gameObject);
                    CurrentSprite.SetActive(false);
                    var info = enemy.Explode();
                    GameObject enemysprite = info.gameObjectOfTheEnemySprite;
                    animal = info.typeOfAnimalOfTheEnemy;
                    //değişim animasyonu
                    var currentSprite = GameObject.Instantiate(enemysprite, transform);
                    currentSprite.transform.localPosition = new(0, 0, enemysprite.transform.localPosition.z);
                    currentSprite.SetActive(true);
                    CurrentSprite = currentSprite;

                    //istatistikleri hayvana göre değiştir.
                    MoveSpeed = AnimalData.Stats[animal].moveSpeed;
                }
                Debug.Log("Hit: " + hit.collider.name);
            }
        }

    }

    public CinemachineCamera virtualCamera;
    private CinemachineConfiner2D confiner;

    private void ChangeCameraToAnotherRoom(Room room)
    {
        if (confiner == null)
        {
            confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
        }
        confiner.BoundingShape2D = room.GetComponent<Collider2D>();
    }
    private void SaveAfterRoomChange(Room room)
    {
        SavePlayer();
    }

    public void SavePlayer()
    {
        Debug.Log("saved player");
        latestSave = new(lastUsedLoader, animal, CurrentSprite);
    }
    public void LoadPlayer()
    {
        readyToCrossRooms = false;
        transform.position = new(latestSave.lastUsedLoader.transform.position.x, latestSave.lastUsedLoader.transform.position.y, transform.position.z);
        animal = latestSave.animal;
        CurrentSprite.SetActive(false);
        var currentSprite = Instantiate(latestSave.spriteUsed, transform);
        currentSprite.transform.localPosition = new(0, 0, latestSave.spriteUsed.transform.localPosition.z);
        currentSprite.SetActive(true);
        if (CurrentSprite != latestSave.spriteUsed)
            Destroy(CurrentSprite);
        CurrentSprite = currentSprite;
        MoveSpeed = AnimalData.Stats[animal].moveSpeed;

    }

    public struct playerSave
    {
        public RoomLoader lastUsedLoader;
        public Animals animal;
        public GameObject spriteUsed;


        public playerSave(RoomLoader lastUsedLoader, Animals animal, GameObject currentSprite) : this()
        {
            this.lastUsedLoader = lastUsedLoader;
            this.animal = animal;
            this.spriteUsed = currentSprite;
        }
    }


}
