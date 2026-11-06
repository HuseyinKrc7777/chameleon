using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    List<RoomLoader> roomLoaders = new();
    public bool _isRoomActive = false;
    void Start()
    {
        if(!TryGetComponent<Collider2D>(out Collider2D col))
        {
            var collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }
        

        foreach (RoomLoader loader in gameObject.GetComponentsInChildren<RoomLoader>())
        {
            roomLoaders.Add(loader);
            loader.Room = this;
        }
        if (_isRoomActive)
        {
            ActivateRoom();
        }
    }

    void Update()
    {
        if (_isRoomActive)
        {
            //oda mantığı
            //düşman kontrolü
            //başka şeyler
            //başka mekankler
        }
    }
    public void ActivateRoom()
    {
        _isRoomActive = true;
        //oda ile ilgli başka kod da olablir
    }
    public void DeActivateRoom()
    {
        _isRoomActive = false;
    }

    

}
