using UnityEngine;
using ChameleonGame;
using System;
using UnityEditor.Media;
using Unity.Cinemachine;
public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 8.0f;
    PlayerInputs _input;
    Rigidbody2D _rigidbody;
    float _speed;
    public bool readyToCrossRooms = true;
    [SerializeField] private Room _currentRoom;
    [SerializeField] private LayerMask InteractLayers;

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
    private void Awake()
    {
        OnRoomChange += ChangeCameraToAnotherRoom;
    }

    void Start()
    {
        _input = GetComponent<PlayerInputs>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Move();
        Interact();
    }
    private void LateUpdate()
    {

    }
    private void Move()
    {
        float targetSpeed = _input.move == Vector2.zero ? 0.0f : MoveSpeed;
        _speed = Mathf.Lerp(_speed, targetSpeed, Time.deltaTime * 10.0f);
        Vector3 inputDirection = new(_input.move.x, _input.move.y, 0);
        Vector3 playerMotion = inputDirection * (_speed * Time.deltaTime);
        Vector3 pos = transform.position;

        transform.position = pos + playerMotion;
        //rigidbody ile de hareket sağlanabilir


    }
    private void Interact()
    {
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
            if(hit.collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                Destroy(transform.Find("Sprite").gameObject);
                Transform enemysprite = enemy.Explode();
                //değişim animasyonu
                enemysprite.parent = transform;
                enemysprite.localPosition = new(0,0,enemysprite.transform.localPosition.z);
            }
            Debug.Log("Hit: " + hit.collider.name);
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


}
