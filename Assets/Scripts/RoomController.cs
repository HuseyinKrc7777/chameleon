using UnityEngine;

public class RoomController : MonoBehaviour
{
    public static RoomController Instance { get; private set; }
    [SerializeField] PlayerController player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeRoom(RoomLoader loader)
    {
        player.CurrentRoom.DeActivateRoom();
        player.transform.position = new(loader.ConnectedRoomLoader.transform.position.x,loader.ConnectedRoomLoader.transform.position.y,player.transform.position.z);
        player.CurrentRoom = loader.ConnectedRoomLoader.Room;
        player.lastUsedLoader = loader.ConnectedRoomLoader;
        player.CurrentRoom.ActivateRoom();
    }
}
