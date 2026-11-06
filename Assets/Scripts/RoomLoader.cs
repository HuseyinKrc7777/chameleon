using UnityEngine;

public class RoomLoader : MonoBehaviour
{
    public Room Room;
    public RoomLoader ConnectedRoomLoader;
    private void OnTriggerEnter2D(Collider2D other) {
        if(!Room._isRoomActive)
            return;
        other.TryGetComponent<PlayerController>(out PlayerController player);
        if(player!=null && player.readyToCrossRooms)
        {
            RoomController.Instance.ChangeRoom(this);
            player.readyToCrossRooms = false;
        }

    }
    private void OnTriggerExit2D(Collider2D other) {
        if(!Room._isRoomActive)
            return;
        other.TryGetComponent<PlayerController>(out PlayerController player);
        if(player!=null && !player.readyToCrossRooms)
        {
            player.readyToCrossRooms = true;
        }
    }
}
