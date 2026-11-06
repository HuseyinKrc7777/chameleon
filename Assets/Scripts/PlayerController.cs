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
    public Room CurrentRoom
    {
        get=>_currentRoom;
        set
        {
            _currentRoom = value;
            OnRoomChange?.Invoke(_currentRoom);
        }
    }
    public RoomLoader lastUsedLoader;
    [SerializeField] private Camera _cam;
    public event Action<Room> OnRoomChange;
    private void Awake() {
        OnRoomChange+=ChangeCameraToAnotherRoom;
    }

    void Start()
    {
        _input = GetComponent<PlayerInputs>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Move();
    }
    private void LateUpdate() {
  
    }
    private void Move()
    {
        float targetSpeed = _input.move == Vector2.zero ? 0.0f : MoveSpeed;
        _speed = Mathf.Lerp(_speed, targetSpeed, Time.deltaTime * 10.0f);
        Vector3 inputDirection =  new(_input.move.x,_input.move.y,0);
        Vector3 playerMotion = inputDirection * (_speed * Time.deltaTime);
        Vector3 pos = transform.position;
        
        transform.position = pos + playerMotion;
        //rigidbody ile de hareket sağlanabilir

        
    }
public CinemachineCamera virtualCamera;
private CinemachineConfiner2D confiner;

    private void ChangeCameraToAnotherRoom(Room room)
    {
        if(confiner==null)
        {
        confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();     
        }
        confiner.BoundingShape2D = room.GetComponent<Collider2D>();    
        }

   
}
