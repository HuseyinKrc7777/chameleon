using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    List<RoomLoader> roomLoaders = new();
    List<Enemy> enemies = new();

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

        foreach(Enemy enemy in gameObject.GetComponentsInChildren<Enemy>())
        {
            enemies.Add(enemy);
            enemy.Room = this;
        }
        if (_isRoomActive)
        {
            ActivateRoom();
        }
        else
        {
            DeActivateRoom();
        }

        
    }
    public void ReloadRoom()
    {
        foreach(Enemy enemy in enemies)
        {
           enemy.ResetEnemy();
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
        
        
        foreach(Enemy en in enemies)
        {
            en.gameObject.SetActive(true);
        }
        //oda ile ilgli başka kod da olablir
    }
    public void DeActivateRoom()
    {
        _isRoomActive = false;
        foreach(Enemy en in enemies)
        {
            en.gameObject.SetActive(false);
        }
    }

    

}
